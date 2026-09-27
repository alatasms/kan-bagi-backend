# Email service functions
# Send email notifications
# Handle email templates
# Process email delivery status 

import datetime
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
from ..models.email_log import EmailLog
from ..config import Config
from ..utils.helpers import generate_otp
import logging

logger = logging.getLogger(__name__)



def send_email_message(to_email, subject, message, is_html=True):
    """
    Sends a general email message to the specified recipient and saves it in the database.

    Args:
        to_email (str): Recipient email address.
        subject (str): Subject of the email.
        message (str): Body of the email.
        is_html (bool): Whether the message is HTML formatted (default True).

    Returns:
        bool: True if the email was processed successfully, False otherwise.
    """
    try:
        if not to_email:
            logger.error("Recipient email address is missing.")
            return False
        if not subject:
            logger.error("Email subject is missing.")
            return False
        if not message:
            logger.error("Email message body is missing.")
            return False

        try:
            success = send_email_main_func(to_email, subject, message, is_html)
            if not success:
                logger.error(f"Failed to send email message to {to_email}")
                return False
        except Exception as e:
            logger.error(f"Error while sending email message: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
            return False

        # Save Email log to database
        try:
            EmailLog(to_email=to_email, type='message', subject=subject, message=message).save()
            logger.info(f"Message log saved successfully for {to_email}")
        except Exception as e:
            logger.error(f"Failed to save message log for {to_email}: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
            return False

        return True
    except Exception as e:
        logger.error(f"Unexpected error in send_email_message: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
        return False


def send_email_main_func(to_email, subject, message, is_html=False):
    """
    Sends an email using the provided recipient email, subject, and message.

    Args:
        to_email (str): Recipient email address.
        subject (str): Subject of the email.
        message (str): Body of the email.
        is_html (bool): Whether the message is HTML formatted.

    Returns:
        bool: True if the email was sent successfully, False otherwise.
    """
    try:
        if not to_email:
            logger.error("Recipient email address is missing.")
            return False
        if not subject:
            logger.error("Email subject is missing.")
            return False
        if not message:
            logger.error("Email message body is missing.")
            return False

        try:
            msg = MIMEText(message, 'html' if is_html else 'plain', 'utf-8')
            msg['Subject'] = subject
            msg['From'] = Config.EMAIL_FROM
            msg['To'] = to_email

            with smtplib.SMTP(Config.EMAIL_SERVER_ADDRESS, Config.EMAIL_PORT_ADDRESS) as server:
                if Config.EMAIL_USE_TLS:
                    try:
                        server.starttls()
                    except Exception as e:
                        logger.error(f"Failed to start TLS: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
                        return False

                # Local test servers accept mail without authentication.
                if Config.EMAIL_SENDER_USERNAME and Config.EMAIL_SENDER_PASSWORD:
                    try:
                        server.login(Config.EMAIL_SENDER_USERNAME, Config.EMAIL_SENDER_PASSWORD)
                    except Exception as e:
                        logger.error(f"Failed to login to email server: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
                        return False

                try:
                    server.send_message(msg)
                except Exception as e:
                    logger.error(f"Failed to send email: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
                    return False

            return True
            
        except Exception as e:
            logger.error(f"Error while preparing or sending email: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
            return False
    except Exception as e:
        logger.error(f"Unexpected error in send_email_main_func: {e}, time: {datetime.datetime.now(datetime.timezone.utc)}")
        return False



def get_email_message_latest(to_email):
    """
    Retrieves the latest general email message log for the specified recipient.

    Args:
        to_email (str): Recipient email address.

    Returns:
        EmailLog or None: The latest email message log if found, otherwise None.
    """
    try:
        if not to_email:
            logger.error("Email address is missing.")
            return None

        result = EmailLog.objects(to_email=to_email, type='message').order_by('-timestamp').first()
        if result:
            return result
        else:
            logger.warning(f"No message logs found for email: {to_email}")
            return None
    except Exception as e:
        logger.error(f"Error fetching latest message log for email {to_email}: {e}")
        return None


def get_email_message_all(to_email):
    """
    Retrieves all general email message logs for the specified recipient.

    Args:
        to_email (str): Recipient email address.

    Returns:
        list: A list of email message logs if found, otherwise an empty list.
    """
    try:
        if not to_email:
            logger.error("Email address is missing.")
            return []

        result = EmailLog.objects(to_email=to_email, type='message').order_by('-timestamp')
        if result:
            return result
        else:
            logger.warning(f"No message logs found for email: {to_email}")
            return []
    except Exception as e:
        logger.error(f"Error fetching all message logs for email {to_email}: {e}")
        return []