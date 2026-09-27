"""E-posta şablonları.

Kullanıcıdan gelen her alan html.escape ile basılır; aksi halde bir ilan açıklamasına yazılan HTML veya
link, bildirim alan herkese giden e-postaya aynen girerdi.
"""
from html import escape

from ..config import Config

APP_NAME = "Kan Bağı"


def _support_link():
    return f'<a href="mailto:{escape(Config.SUPPORT_EMAIL)}">{escape(Config.SUPPORT_EMAIL)}</a>'


def _footer():
    text = "Bu e-posta otomatik olarak gönderilmiştir. Lütfen doğrudan yanıt vermeyin."
    if Config.SUPPORT_EMAIL:
        text += f" Sorularınız için {_support_link()} adresine yazın."
    return text


def _layout(title, header_color, body_html):
    return f"""<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f4f4f4; }}
        .container {{ max-width: 600px; margin: 20px auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
        .header {{ background: {header_color}; color: #fff; padding: 10px; text-align: center; border-radius: 8px 8px 0 0; }}
        .content {{ padding: 20px; }}
        .content p {{ margin: 10px 0; }}
        .details {{ background: #f9f9f9; padding: 15px; border-left: 4px solid {header_color}; margin: 10px 0; }}
        .footer {{ text-align: center; font-size: 12px; color: #777; padding: 10px; border-top: 1px solid #ddd; }}
        a {{ color: {header_color}; text-decoration: none; }}
        a:hover {{ text-decoration: underline; }}
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h2>{title}</h2>
        </div>
        <div class="content">
{body_html}
            <p>Saygılarımızla,<br>{APP_NAME} Ekibi</p>
        </div>
        <div class="footer">
            <p>{_footer()}</p>
        </div>
    </div>
</body>
</html>"""


def _post_details(post):
    hospital = post.get("hospital") or {}
    return f"""            <div class="details">
                <h3>Talep Detayları</h3>
                <p><strong>Hasta Adı:</strong> {escape(str(post.get('patientFullName', '')))}</p>
                <p><strong>Kan Grubu:</strong> {escape(str(post.get('bloodType', '')))}</p>
                <p><strong>Hastane:</strong> {escape(str(hospital.get('hospitalName', '')))}</p>
                <p><strong>Adres:</strong> {escape(str(hospital.get('hospitalAddress', '')))}</p>
                <p><strong>İletişim Numarası:</strong> {escape(str(post.get('phoneNumbers', '')))}</p>
                <p><strong>Açıklama:</strong> {escape(str(post.get('description', '')))}</p>
            </div>"""


def welcome(name):
    subject = f"🎉 {APP_NAME}'na Hoş Geldiniz!"
    contact = f" Herhangi bir sorunuz olursa, bizimle {_support_link()} adresinden iletişime geçebilirsiniz. ✉️" if Config.SUPPORT_EMAIL else ""
    body = f"""            <p>Merhaba {escape(name)},</p>
            <p>Sisteme kaydolduğunuz için teşekkür ederiz! 😊 Size yardımcı olmaktan mutluluk duyarız!</p>
            <p>Artık {APP_NAME} ile kan bağışı taleplerini takip edebilir, destek sağlayabilir ve topluluğumuza katkıda bulunabilirsiniz.{contact}</p>
            <p>Teşekkürler!</p>"""
    return subject, _layout("Hoş Geldiniz! 🎉", "#0078d4", body)


def new_post(recipient_name, post):
    """Kan ihtiyacı bildirimi. recipient_name bildirimi alan kişidir, ilan sahibi değil."""
    hospital = post.get("hospital") or {}
    hospital_name = escape(str(hospital.get("hospitalName", "")))
    subject = f"{hospital.get('hospitalName', '')} - {post.get('bloodType', '')} Kan İhtiyacı Talebi"
    body = f"""            <p>Merhaba {escape(recipient_name)},</p>
            <p>{hospital_name} ({escape(str(hospital.get('hospitalAddress', '')))}) adresinde {escape(str(post.get('patientAge', '')))} yaşındaki bir hasta için <strong>{escape(str(post.get('bloodType', '')))}</strong> kan grubuna acil ihtiyaç bulunmaktadır. Bildirim tercihlerinize uyduğu için size haber veriyoruz.</p>
{_post_details(post)}
            <p><strong>İlan sahibi:</strong> {escape(str(post.get('ownerName', '')))} {escape(str(post.get('ownerSurname', '')))}</p>
            <p>Yardımcı olabilirseniz {APP_NAME} uygulaması üzerinden ilan sahibiyle iletişime geçebilir ya da yukarıdaki numarayı arayabilirsiniz.</p>"""
    return subject, _layout("Kan İhtiyacı Talebi", "#d32f2f", body)


def post_expired(post):
    hospital = post.get("hospital") or {}
    subject = f"Kan İhtiyacı Talebiniz Süresi Doldu - {hospital.get('hospitalName', '')}"
    body = f"""            <p>Sayın {escape(str(post.get('ownerName', '')))} {escape(str(post.get('ownerSurname', '')))},</p>
            <p>{APP_NAME} üzerinde oluşturduğunuz kan ihtiyacı talebinin süresi dolmuştur. Talebiniz artık aktif değildir.</p>
{_post_details(post)}
            <p>Eğer kan ihtiyacınız devam ediyorsa, uygulamamız üzerinden talebinizi kolayca yeniden aktif hale getirebilirsiniz.</p>"""
    return subject, _layout("Kan İhtiyacı Talebi Süresi Doldu", "#ff9800", body)
