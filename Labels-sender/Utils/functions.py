import pandas as pd
import qrcode
from fpdf import FPDF
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.mime.base import MIMEBase
from email import encoders

# Read excel fail
def read_excel(path, sheet_name):
    # Leer el archivo Excel
    df = pd.read_excel(path, sheet_name=sheet_name)
    return df

# Generate qr code
def generate_qr(code, instrucciones_entrada, otros_ingredientes):
    qr_data = "Otros ingredientes: " +otros_ingredientes+ "\nInstrucciones de entrada:" +instrucciones_entrada+ ""

    qr = qrcode.QRCode(
        version=10,
        error_correction=qrcode.constants.ERROR_CORRECT_H,
        box_size=20,
        border=4,
    )
    qr.add_data(qr_data)
    qr.make(fit=True)
    img = qr.make_image(fill='black', back_color='white')
    path=str(code)+"_qr.png"
    img.save(path)
    return path

# Create individual label
def create_indvidual_label(pdf, x, y, category, style, code, qr_path):
    pdf.set_xy(x-2, y-4)
    pdf.set_font("Arial", style='B', size=11)  # Texto en negrita para la primera celda
    pdf.cell(0, 8, "Concurso homebrewer Córdoba", ln=True, align='L')
    pdf.set_xy(x-2, y + 3)
    pdf.set_font("Arial", size=10)  # Texto en negrita para la primera celda
    pdf.cell(0, 8, category, ln=True, align='L')
    pdf.set_xy(x-2, y + 9)
    pdf.cell(0, 8, style, ln=True, align='L')
    pdf.set_xy(x-2, y + 13)
    pdf.cell(0, 10, "Entrada: " + str(code), ln=True, align='L')
    pdf.image(qr_path, x=x+9, y=y+21, w=44)
    pdf.rect(x-5, y-5, 70, 70)
    
# Create pdf with three individual labels
def create_pdf_with_label(category, style, code, qr_path):
    # Crear instancia de la clase FPDF
    pdf = FPDF()

    # Añadir una página
    pdf.add_page()

    # Establecer fuente
    pdf.set_font("Arial", size=10)

    # Crear tres etiquetas

    create_indvidual_label(pdf, 30, 20, category, style, code, qr_path)
    create_indvidual_label(pdf, 30, 100, category, style, code, qr_path)
    create_indvidual_label(pdf, 30, 180, category, style, code, qr_path)

    file_name=str(code)+".pdf"
    pdf.output(file_name)
    
    return file_name

def send_email(server, email_from, to, name, pdf_path):
    msg = MIMEMultipart()
    msg['From'] = email_from
    msg['To'] = to
    msg['Subject'] = 'Etiquetas Concurso Homebrewer Córdoba'

    # Contenido del correo
    body = f"Hola, " +name+ "\n\nMuchas gracias por participar en el IX Concurso Homebrewer de Córdoba. Para asegurar que todo transcurra de manera organizada, las botellas irán identificadas unicamente con la etiqueta recibida como archivo adjunto en este email. La etiqueta debe ir sujeta a la botella mediante una goma elástica u otro método sencillo de quitar, no deberá ir pegada con pegamentos ni sujeta con cintas adhesivas.\n \n Recuerda, la fecha de entrega será del 10 al 17 de noviembre en la siguiente dirección: \n Hotel Casa de los Azulejos \n C/ Fernando Colón 5, \n 14002 Córdoba \n (Indicar en el paquete: Concurso Homebrewer). \n\n\n Muchas gracias, \n Saludos."
    msg.attach(MIMEText(body, 'plain'))

    # Attach labels
    with open(pdf_path, "rb") as attach:
        parte = MIMEBase('application', 'octet-stream')
        parte.set_payload(attach.read())
        encoders.encode_base64(parte)
        parte.add_header('Content-Disposition', f"attachment; filename= {pdf_path}")
        msg.attach(parte)
        
    # Enviar el correo
    server.sendmail(email_from, to, msg.as_string())