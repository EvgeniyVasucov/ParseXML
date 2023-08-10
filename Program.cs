﻿using System.Xml;
using System.Xml.Serialization;

public class Program
{
    static void Main()
    {
        string path = @"D:\SomeFile\UT_ZVREGBENZ_5252_5252_5252000777525201001_20210125_7F167963-6BF9-494E-90BB-461759218EY1.xml";
        FileInfo fileInfo = new FileInfo(path); // создаем объект для работы с файлами по указанному пути
        if (fileInfo.Exists) // определяем, существует ли каталог
        {
            Console.WriteLine($"Имя файла: {fileInfo.Name}");
            Console.WriteLine($"Дата получения: {fileInfo.CreationTime:d}");

            XmlSerializer formatter = new XmlSerializer(typeof(File)); // передаем в конструктор тип класса Файл

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate)) // восстановление из файла
            {
                File? newelements = formatter.Deserialize(fs) as File;
                if (newelements != null)
                {
                    ConverteHelper GetDate = new ConverteHelper(); // создаем объект для вызова метода получения даты
                    Console.WriteLine($"ИдФайл: {newelements.idFile}");
                    Console.WriteLine($"ВерсПрог: {newelements.programVersion}");
                    Console.WriteLine($"ВерсФорм: {newelements.formatVersion}");
                    Console.WriteLine($"КНД: {newelements.Document.codeKnd}");
                    GetDate.ObjectToDate(newelements.Document.docDate);
                    Console.WriteLine($"КодНО: {newelements.Document.codeNO}");
                    Console.WriteLine($"МестоНахож: {newelements.Document.SvNP.location}");
                    Console.WriteLine($"АдрОсущ: {newelements.Document.SvNP.address}");
                    if (newelements.Document.SvNP.NPUL.fullname != "Undefined")
                    {
                        Console.WriteLine($"НаимОрг: {newelements.Document.SvNP.NPUL.fullname}");
                        Console.WriteLine($"НаимОргСокр: {newelements.Document.SvNP.NPUL.shortname}");
                        Console.WriteLine($"ИННЮЛ: {newelements.Document.SvNP.NPUL.innUL}");
                        Console.WriteLine($"КПП: {newelements.Document.SvNP.NPUL.kpp}");
                    }
                    else
                    {
                        Console.WriteLine($"ИННФЛ: {newelements.Document.SvNP.NPFL.innFL}");
                        Console.WriteLine($"Фамилия: {newelements.Document.SvNP.NPFL.FIO.lastname}");
                        Console.WriteLine($"Имя: {newelements.Document.SvNP.NPFL.FIO.firstname}");
                        Console.WriteLine($"Отчество: {newelements.Document.SvNP.NPFL.FIO.secondname}");
                    }
                    Console.WriteLine($"ПрПодп: {newelements.Document.Signed.signedType}");
                    Console.WriteLine($"Тлф: {newelements.Document.Signed.phone}");
                    Console.WriteLine($"Фамилия: {newelements.Document.Signed.FIO.lastname}");
                    Console.WriteLine($"Имя: {newelements.Document.Signed.FIO.firstname}");
                    Console.WriteLine($"Отчество: {newelements.Document.Signed.FIO.secondname}");
                    Console.WriteLine($"НаимДок: {newelements.Document.Signed.SvPred.docName}");
                    Console.WriteLine($"ВидДеят: {newelements.Document.Statement.declaredType}");
                    if (newelements.Document.Statement.declaredType == 1)
                    {
                        for (int i = 0; i < newelements.Document.Statement.SvedProizv.Count; i++)
                        {
                            GetDate.ObjectToDate(newelements.Document.Statement.SvedProizv[i].statusDate);
                            Console.WriteLine($"АдрФакт: {newelements.Document.Statement.SvedProizv[i].SvedProizvData.address}");
                            Console.WriteLine($"КППОб: {newelements.Document.Statement.SvedProizv[i].SvedProizvData.kpp}");
                            Console.WriteLine($"АдрМощ: {newelements.Document.Statement.SvedProizv[i].SvedProizvData.addressProduction}");
                            Console.WriteLine($"КППМощ: {newelements.Document.Statement.SvedProizv[i].SvedProizvData.kppProduction}");
                            Console.WriteLine($"ДокПрав: {newelements.Document.Statement.SvedProizv[i].SvedProizvData.document}");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < newelements.Document.Statement.SvedPer.Count; j++)
                        {
                            GetDate.ObjectToDate(newelements.Document.Statement.SvedPer[j].statusDate);
                            Console.WriteLine($"АдрФакт: {newelements.Document.Statement.SvedPer[j].SvedPerData.address}");
                            Console.WriteLine($"КППОб: {newelements.Document.Statement.SvedPer[j].SvedPerData.kpp}");
                            Console.WriteLine($"АдрМощ: {newelements.Document.Statement.SvedPer[j].SvedPerData.addressProduction}");
                            Console.WriteLine($"КППМощ: {newelements.Document.Statement.SvedPer[j].SvedPerData.kppProduction}");
                            Console.WriteLine($"ДокПрав: {newelements.Document.Statement.SvedPer[j].SvedPerData.document}");
                        }
                    }
                }
            }
        }
        else
            Console.WriteLine("NOT File!");
    }
}
public class ConverteHelper
{
    public void ObjectToDate(string dateValue)
    {
        try
        {
            DateTime GetdateValue;
            var dateMass = dateValue.Split('.');
            GetdateValue = new DateTime(Convert.ToInt32(dateMass[2]), Convert.ToInt32(dateMass[1]), Convert.ToInt32(dateMass[0]));

            if (GetdateValue <= DateTime.Now && GetdateValue > DateTime.Now.AddYears(-30))
            {
                Console.WriteLine($"Дата: {GetdateValue:d}");
 
            }
            else
                Console.WriteLine("Дата: Undefined");
        }
        catch (Exception)
        {
            Console.WriteLine("Дата: Undefined");
        }
    }
}

[XmlRoot(ElementName = "Файл")]
public class File
{
    [XmlElement(ElementName = "Документ")]
    public Document Document { get; set; } = new Document();
    [XmlAttribute("ИдФайл")]
    public string idFile { get; set; } = "Undefined";
    [XmlAttribute("ВерсПрог")]
    public string programVersion { get; set; } = "Undefined";
    [XmlAttribute("ВерсФорм")]
    public string formatVersion { get; set; } = "5.01";
}
public class Document
{
    [XmlAttribute("КНД")]
    public int codeKnd { get; set; } = 1111060;
    [XmlAttribute("ДатаДок")]
    public string docDate { get; set; } = "Undefined";
    [XmlAttribute("КодНО")]
    public short codeNO { get; set; }
    [XmlElement(ElementName = "СвНП")]
    public SvNP SvNP { get; set; } = new SvNP();
    [XmlElement(ElementName = "Подписант")]
    public Signed Signed { get; set; } = new Signed();
    [XmlElement(ElementName = "ЗаявПрямБен")]
    public Statement Statement { get; set; } = new Statement();
}
public class SvNP
{
    [XmlAttribute("МестоНахож")]
    public string location { get; set; } = "Undefined";
    [XmlAttribute("АдрОсущ")]
    public string address { get; set; } = "Undefined";
    [XmlElement(ElementName = "НПЮЛ")]
    public NPUL NPUL { get; set; } = new NPUL();
    [XmlElement(ElementName = "НПФЛ")]
    public NPFL NPFL { get; set; } = new NPFL();
}
public class NPUL
{
    [XmlAttribute("НаимОрг")]
    public string fullname { get; set; } = "Undefined";
    [XmlAttribute("НаимОргСокр")]
    public string shortname { get; set; } = "Undefined";
    [XmlAttribute("ИННЮЛ")]
    public long innUL { get; set; }
    [XmlAttribute("КПП")]
    public int kpp { get; set; }
}
public class NPFL
{
    [XmlAttribute("ИННФЛ")]
    public long innFL { get; set; }
    [XmlElement(ElementName = "ФИО")]
    public FIO FIO { get; set; } = new FIO();
}
public class Signed
{
    [XmlAttribute("ПрПодп")]
    public byte signedType { get; set; }
    [XmlAttribute("Тлф")]
    public string phone { get; set; } = "Undefined";
    [XmlElement(ElementName = "ФИО")]
    public FIO FIO { get; set; } = new FIO();
    [XmlElement(ElementName = "СвПред")]
    public SvPred SvPred { get; set; } = new SvPred();
}
public class FIO
{
    [XmlAttribute("Фамилия")]
    public string lastname { get; set; } = "Undefined";
    [XmlAttribute("Имя")]
    public string firstname { get; set; } = "Undefined";
    [XmlAttribute("Отчество")]
    public string secondname { get; set; } = "Undefined";
}
public class SvPred
{
    [XmlAttribute("НаимДок")]
    public string docName { get; set; } = "Undefined";
}
public class Statement
{
    [XmlAttribute("ВидДеят")]
    public byte declaredType { get; set; }
    [XmlElement(ElementName = "СведПроизв")]
    public List<SvedProizv> SvedProizv { get; set; } = new List<SvedProizv>();
    [XmlElement(ElementName = "СведПер")]
    public List<SvedPer> SvedPer { get; set; } = new List<SvedPer>();
}
public class SvedProizv
{
    [XmlAttribute("ПоСост")]
    public string statusDate { get; set; } = "Undefined";
    [XmlElement(ElementName = "СведПроизвДата")]
    public SvedProizvData SvedProizvData { get; set; } = new SvedProizvData();
}
public class SvedPer
{
    [XmlAttribute("ПоСост")]
    public string statusDate { get; set; } = "Undefined";
    [XmlElement(ElementName = "СведПерДата")]
    public SvedPerData SvedPerData { get; set; } = new SvedPerData();
}
public class SvedProizvData
{
    [XmlAttribute("АдрФакт")]
    public string address { get; set; } = "Undefined";
    [XmlAttribute("КППОб")]
    public string kpp { get; set; } = "Undefined";
    [XmlAttribute("АдрМощ")]
    public string addressProduction { get; set; } = "Undefined";
    [XmlAttribute("КППМощ")]
    public string kppProduction { get; set; } = "Undefined";
    [XmlAttribute("ДокПрав")]
    public string document { get; set; } = "Undefined";
}
public class SvedPerData
{
    [XmlAttribute("АдрФакт")]
    public string address { get; set; } = "Undefined";
    [XmlAttribute("КППОб")]
    public string kpp { get; set; } = "Undefined";
    [XmlAttribute("АдрМощ")]
    public string addressProduction { get; set; } = "Undefined";
    [XmlAttribute("КППМощ")]
    public string kppProduction { get; set; } = "Undefined";
    [XmlAttribute("ДокПрав")]
    public string document { get; set; } = "Undefined";
}
