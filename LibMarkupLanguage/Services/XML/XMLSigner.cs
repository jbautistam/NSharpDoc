using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Clase de firma de un archivo XML
	/// </summary>
	public class XMLSigner
	{
		/// <summary>
		///		Firma un archivo XML
		/// </summary>
		public XmlDocument SignXMLFile(string strFileName, X509Certificate2 objCertificate, string strReferenceToSign = "")
		{ XmlDocument objXMLDocument = new XmlDocument();
 
				// Carga el XML
					objXMLDocument.Load(strFileName);
				// Firma el documento
					SignXmlDocument(objXMLDocument, objCertificate, strReferenceToSign);
				// Devuelve el documento firmado
					return objXMLDocument;
		}
 
		/// <summary>
		///		Firma un texto XML
		/// </summary>
    public XmlDocument SignXmlText(string strXml, X509Certificate2 objCertificate, string strReferenceToSign = "")
    { XmlDocument objXMLDocument = new XmlDocument();
 
				// Indica que se mantengan los espacios y saltos de línea
					objXMLDocument.PreserveWhitespace = true;
				// Carga el XML
					objXMLDocument.LoadXml(strXml);
				// Firma el documento
					SignXmlDocument(objXMLDocument, objCertificate, strReferenceToSign);
				// Devuelve el documento
					return objXMLDocument;
    }
 
    /// <summary>
    ///		Firma un archivo XML con un certificado
    /// </summary>
		public void SignXmlDocument(XmlDocument objXMLDocument, X509Certificate2 objCertificate, string strReferenceToSign = "")
		{	SignedXml objSignedXml = new SignedXml(objXMLDocument);

				// Añade la clave al documento SignedXml
					objSignedXml.SigningKey = (RSACryptoServiceProvider) objCertificate.PrivateKey;
				// Asigna el identificador de referencia
					if (!string.IsNullOrWhiteSpace(strReferenceToSign))
						objSignedXml.AddReference(GetReference(strReferenceToSign));
				// Añade la información de los parámetros de firma
					objSignedXml.Signature.KeyInfo = GetKeyInfoFromCertificate(objCertificate);
				// Calcula la firma
					objSignedXml.ComputeSignature();
				// Añade el elemento firmado al documento XML
					objXMLDocument.DocumentElement.AppendChild(objXMLDocument.ImportNode(objSignedXml.GetXml(), true));
		}

		/// <summary>
		///		Obtiene los datos de la referencia
		/// </summary>
		private Reference GetReference(string strReferenceToSign)
		{	Reference objXmlReference = new Reference();

				// Crea una referencia a firmar
					objXmlReference.Uri = "#" + strReferenceToSign;
				// Añade una transformación a la referencia
					objXmlReference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
				// Devuelve la referencia creada
					return objXmlReference;
		}

		/// <summary>
		///		Obtiene la información de la firma asociada al certificado digital
		/// </summary>
		private KeyInfo GetKeyInfoFromCertificate(X509Certificate2 objCertificate)
		{	KeyInfo objKeyInfo = new KeyInfo();

				// Añade la cláusula con el certificado
					objKeyInfo.AddClause(new KeyInfoX509Data(objCertificate));
				// Devuelve la información
					return objKeyInfo;
		}
	}
}