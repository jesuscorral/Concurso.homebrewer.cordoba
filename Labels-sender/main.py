import smtplib

from Utils.functions import read_excel
from Utils.functions import generate_qr
from Utils.functions import create_pdf_with_label
from Utils.functions import send_email

path = "C:\MyPersonalWS\Concurso.homebrewer.cordoba\Labels-sender\participantes.xlsx"
sheet_name="Sheet1"

# Read excel file
participants = read_excel(path, sheet_name)

# TODO: Configure SMTP Server 

# SMTP Server configuration
smtp_server = 'smtp.gmail.com'
smtp_port = 465
smtp_user = 'xxx@gmail.com'
smtp_password = 'xxx'

# Connect to SMTP Server configuration
server = smtplib.SMTP_SSL(smtp_server, smtp_port)
server.login(smtp_user, smtp_password)

# TODO: Review header from excel file

for index, row in participants.iterrows():
    # Get data from Excel file
    code = row['Code']
    instrucciones_entrada = row['Instrucciones_entrada']
    otros_ingredientes = row['Otros_ingredientes']
    category = row['Categoria']
    style = row['Estilo']
    email = row['Email']
    name = row['Nombre']
    
    # Generate QR image
    qr_path_by_particpant = generate_qr(code, instrucciones_entrada=instrucciones_entrada, otros_ingredientes=otros_ingredientes)
    
    # Create 3 labels in the same pdf file
    pdf_path = create_pdf_with_label(category, style, code, qr_path_by_particpant)
    
    send_email(server, smtp_user, email, name, pdf_path)
    
# Close SMTP Server connection
server.quit()
