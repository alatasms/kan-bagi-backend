import pika
import json
import logging
import time
from app.services.email_service import send_email_message
from mongoengine import connect
from app.config import Config
from app.models.message_log import MessageLog
from app.notifications import subscribers
from app.templates import emails

# RabbitMQ bağlantı URI'si
RABBITMQ_URI_ADDRESS = Config.RABBITMQ_URI_ADDRESS

# Each queue is bound to the fanout exchange the .NET services publish to (see docs/events.md).
# Declaring and binding here means a fresh RabbitMQ needs no manual setup.
QUEUE_NAME_REGISTERED = "user-registered-queue"
QUEUE_NAME_POST_CREATED = "new-post-created-queue"
QUEUE_NAME_POST_EXPIRED = "user-post-expired-queue"
QUEUE_NAME_PREFERENCES = "notification-preferences-changed-queue"

BINDINGS = {
    QUEUE_NAME_REGISTERED: "user-registered",
    QUEUE_NAME_POST_CREATED: "new-post-created",
    QUEUE_NAME_POST_EXPIRED: "user-post-expired",
    QUEUE_NAME_PREFERENCES: "notification-preferences-changed",
}

# Logging configuration
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)


def handle_post_created(message):
    """Email every matching subscriber about a new post. Returns an error string or None."""
    post = message["message"]
    recipients = subscribers.email_recipients_for_post(post)
    if not recipients:
        logger.info(f"İlan {post.get('postId')} için bildirim alacak abone yok.")
        return None

    failed = []
    for subscriber in recipients:
        subject, body = emails.new_post(subscriber.name, post)
        if send_email_message(subscriber.email, subject, body):
            logger.info(f"Kan talebi e-postası gönderildi: kullanıcı {subscriber.user_id}")
        else:
            failed.append(subscriber.user_id)
    return f"Email sending failed for {len(failed)} of {len(recipients)} recipients" if failed else None


def handle_registered(message):
    data = message["message"]
    name = data.get("name")
    email_address = data.get("email")
    if not name or not email_address:
        return "Name or email missing"
    subject, body = emails.welcome(name)
    return None if send_email_message(email_address, subject, body) else "Email sending failed"


def handle_post_expired(message):
    """Tell the owner their post expired. The owner's address comes from the subscriber list."""
    post = message["message"]
    owner = subscribers.find(post.get("ownerId", ""))
    if owner is None or not owner.email:
        return f"No email address known for owner {post.get('ownerId')}"
    subject, body = emails.post_expired(post)
    return None if send_email_message(owner.email, subject, body) else "Email sending failed"


def handle_preferences_changed(message):
    subscribers.upsert_from_preferences_event(message["message"])
    return None


HANDLERS = {
    QUEUE_NAME_REGISTERED: handle_registered,
    QUEUE_NAME_POST_CREATED: handle_post_created,
    QUEUE_NAME_POST_EXPIRED: handle_post_expired,
    QUEUE_NAME_PREFERENCES: handle_preferences_changed,
}


def process(queue_name, ch, method, body):
    """
    Log, handle and acknowledge one message. The message is acknowledged only after it was handled,
    so a crash mid-way redelivers it instead of losing it. Messages that fail are logged and dropped
    rather than requeued forever.
    """
    message_log = None
    try:
        message = json.loads(body)
        message_id = message.get("messageId")

        # Delivery is at-least-once; a message already handled successfully is not handled (emailed) again.
        if message_id and MessageLog.objects(queue_name=queue_name, message_id=message_id, status='processed').first():
            logger.info(f"{queue_name}: {message_id} zaten işlendi, atlanıyor.")
            return

        message_log = MessageLog(queue_name=queue_name, message_id=message_id, message_data=message)
        message_log.save()

        error = HANDLERS[queue_name](message)
        message_log.status = 'failed' if error else 'processed'
        message_log.error = error
        message_log.save()
        if error:
            logger.error(f"{queue_name}: {error}")
    except Exception as e:
        logger.error(f"{queue_name} mesajı işlenirken hata oluştu: {e}")
        if message_log is not None:
            message_log.status = 'failed'
            message_log.error = str(e)
            message_log.save()
    finally:
        ch.basic_ack(delivery_tag=method.delivery_tag)


class RabbitMQConsumer:
    def __init__(self):
        self._connection = None
        self._channel = None
        self._closing = False
        self._consumer_tags = []
        self._reconnect_delay = 5  # Initial delay in seconds
        self._max_reconnect_delay = 300  # Maximum delay in seconds (5 minutes)

    def _maybe_reconnect(self):
        """Yeniden bağlanma denemesi"""
        if not self._closing:
            retry_count = getattr(self, '_retry_count', 0) + 1
            setattr(self, '_retry_count', retry_count)

            logger.warning(f"RabbitMQ bağlantısı koptu. {retry_count}. deneme yapılacak. "
                         f"{self._reconnect_delay} saniye bekleniyor...")

            time.sleep(self._reconnect_delay)

            # Exponential backoff with maximum delay
            self._reconnect_delay = min(self._reconnect_delay * 2, self._max_reconnect_delay)

            logger.info(f"Yeniden bağlanma girişimi #{retry_count} - "
                       f"Sonraki deneme {self._reconnect_delay} saniye sonra yapılacak "
                       f"(Max: {self._max_reconnect_delay} saniye)")

    def connect(self):
        """RabbitMQ'ya bağlanma"""
        while not self._closing:
            try:
                parameters = pika.URLParameters(RABBITMQ_URI_ADDRESS)
                self._connection = pika.BlockingConnection(parameters)
                self._channel = self._connection.channel()
                self._channel.basic_qos(prefetch_count=10)

                # Exchanges are declared with the same settings MassTransit uses, so either side may create them first.
                self._consumer_tags = []
                for queue_name, exchange in BINDINGS.items():
                    self._channel.exchange_declare(exchange=exchange, exchange_type='fanout', durable=True)
                    self._channel.queue_declare(queue=queue_name, durable=True)
                    self._channel.queue_bind(queue=queue_name, exchange=exchange)
                    self._consumer_tags.append(
                        self._channel.basic_consume(
                            queue=queue_name,
                            on_message_callback=lambda ch, method, properties, body, q=queue_name: process(q, ch, method, body),
                            auto_ack=False
                        )
                    )

                # Bağlantı başarılı olduğunda retry sayacını sıfırla
                setattr(self, '_retry_count', 0)
                self._reconnect_delay = 5  # Reset delay to initial value

                logger.info("RabbitMQ bağlantısı başarıyla kuruldu! Servis normal çalışmaya devam ediyor.")
                return True
            except pika.exceptions.AMQPConnectionError as e:
                logger.error(f"RabbitMQ bağlantısı başarısız: {e}")
                self._maybe_reconnect()
            except Exception as e:
                logger.error(f"Beklenmeyen hata: {e}")
                self._maybe_reconnect()

    def run(self):
        """Consumer'ı çalıştır"""
        try:
            # MongoDB bağlantısı
            connect(db=Config.MONGODB_NOTIFICATION_DB_NAME, host=Config.MONGODB_URI_ADDRESS)
            logger.info("MongoDB bağlantısı başarılı")

            # RabbitMQ bağlantısı ve consumer başlatma
            while not self._closing:
                try:
                    self.connect()
                    logger.info("Mesajlar bekleniyor...")
                    self._channel.start_consuming()
                except KeyboardInterrupt:
                    self._closing = True
                    if self._channel:
                        self._channel.stop_consuming()
                    break
                except Exception as e:
                    logger.error(f"Consumer hatası: {e}")
                    # Kanalı ve bağlantıyı temizle
                    if self._channel:
                        try:
                            self._channel.close()
                        except Exception:
                            pass
                    if self._connection and not self._connection.is_closed:
                        try:
                            self._connection.close()
                        except Exception:
                            pass
                    self._channel = None
                    self._connection = None
                    # Yeniden bağlanmayı dene
                    self._maybe_reconnect()

        except KeyboardInterrupt:
            logger.info("Consumer kapatılıyor...")
            self.stop()
        except Exception as e:
            logger.error(f"Beklenmeyen hata: {e}")
            self.stop()

    def stop(self):
        """Consumer'ı durdur"""
        self._closing = True
        if self._channel:
            for tag in self._consumer_tags:
                try:
                    self._channel.basic_cancel(tag)
                except Exception:
                    pass
            try:
                self._channel.close()
            except Exception:
                pass
        if self._connection and not self._connection.is_closed:
            try:
                self._connection.close()
            except Exception:
                pass


def consume_messages():
    """RabbitMQ consumer'ı başlat"""
    consumer = RabbitMQConsumer()
    consumer.run()


if __name__ == "__main__":
    consume_messages()
