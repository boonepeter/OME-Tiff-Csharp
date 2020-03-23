using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using OMETiffCsharp.OMETiff;
using System.IO;
using System.Text;

namespace OMETest
{
    [TestClass]
    public class ReadingTest
    {
        private const string META = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
            "<OME xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" UUID=\"urn:uuid:cb8b431f-ab4e-4e54-8351-96d4eb785014\"" +
            " Creator=\"OME Bio-Formats 5.2.0-m4\" xmlns=\"http://www.openmicroscopy.org/Schemas/OME/2016-06\">\r\n" +
            "  <!--Warning: this comment is an OME-XML metadata block, which contains crucial dimensional parameters and other important metadata. Please edit cautiously (if at all), and back up the original data before doing so. For more information, see the OME-TIFF web site: https://docs.openmicroscopy.org/latest/ome-model/ome-tiff/. -->\r\n" +
            "  <Experiment Type=\"PGIDocumentation\" ID=\"urn:lsid:loci.wisc.edu:Experiment:OWS348\">\r\n" +
            "    <Description>4 Cell Embryo</Description>\r\n" +
            "    <ExperimenterRef ID=\"urn:lsid:loci.wisc.edu:Experimenter:116\" />\r\n" +
            "  </Experiment>\r\n" +
            "  <Experimenter ID=\"urn:lsid:loci.wisc.edu:Experimenter:116\" FirstName=\"Maimoon\" LastName=\"Nasim\" Email=\"mnasim@wisc.edu\" Institution=\" \" />\r\n" +
            "  <Instrument ID=\"urn:lsid:loci.wisc.edu:Instrument:OWS1\">\r\n" +
            "    <Microscope Manufacturer=\"Nikon\" Model=\"Eclipse TE300\" SerialNumber=\"U629762\" Type=\"Inverted\" />\r\n" +
            "    <Laser Manufacturer=\"Spectral Physics\" Model=\"Tsunami 5W\" SerialNumber=\"2123\" ID=\"urn:lsid:loci.wisc.edu:LightSource:OWS1\" Type=\"SolidState\" LaserMedium=\"TiSapphire\" />\r\n" +
            "    <Detector Manufacturer=\"Hamamatzu\" Model=\"H7422\" ID=\"urn:lsid:loci.wisc.edu:Detector:OWS1\" Type=\"PMT\" />\r\n" +
            "    <Detector Manufacturer=\"Bio-Rad\" Model=\"1024TLD\" ID=\"urn:lsid:loci.wisc.edu:Detector:OWS2\" Type=\"Photodiode\" />\r\n" +
            "    <Objective Manufacturer=\"Nikon\" Model=\"S Fluor\" SerialNumber=\"044989\" ID=\"urn:lsid:loci.wisc.edu:Objective:OWS2\" Correction=\"PlanApo\" Immersion=\"Oil\" LensNA=\"1.4\" NominalMagnification=\"100\" CalibratedMagnification=\"100\" WorkingDistance=\"0.13\" />\r\n" +
            "  </Instrument>\r\n" +
            "  <Image ID=\"Image:0\" Name=\"tubhiswt\">\r\n" +
            "    <AcquisitionDate>2013-01-15T17:02:40</AcquisitionDate>\r\n" +
            "    <ExperimenterRef ID=\"urn:lsid:loci.wisc.edu:Experimenter:116\" />\r\n" +
            "    <Pixels ID=\"Pixels:0\" DimensionOrder=\"XYZTC\" Type=\"uint8\" SignificantBits=\"8\" Interleaved=\"false\" BigEndian=\"false\" SizeX=\"512\" SizeY=\"512\" SizeZ=\"1\" SizeC=\"2\" SizeT=\"1\">\r\n" +
            "      <Channel ID=\"Channel:0:0\" SamplesPerPixel=\"1\">\r\n" +
            "        <LightSourceSettings ID=\"urn:lsid:loci.wisc.edu:LightSource:OWS1\" />\r\n" +
            "        <DetectorSettings ID=\"urn:lsid:loci.wisc.edu:Detector:OWS1\" />\r\n" +
            "        <LightPath />\r\n" +
            "      </Channel>\r\n" +
            "      <Channel ID=\"Channel:0:1\" SamplesPerPixel=\"1\">\r\n" +
            "        <LightSourceSettings ID=\"urn:lsid:loci.wisc.edu:LightSource:OWS1\" />\r\n" +
            "        <DetectorSettings ID=\"urn:lsid:loci.wisc.edu:Detector:OWS2\" />\r\n" +
            "        <LightPath />\r\n" +
            "      </Channel>\r\n" +
            "      <TiffData PlaneCount=\"1\">\r\n" +"" +
            "        <UUID FileName=\"tubhiswt_C0.ome.tif\">urn:uuid:cb8b431f-ab4e-4e54-8351-96d4eb785014</UUID>\r\n" +
            "      </TiffData>\r\n" +
            "      <TiffData FirstC=\"1\" PlaneCount=\"1\">\r\n" +
            "        <UUID FileName=\"tubhiswt_C1.ome.tif\">urn:uuid:85a5ab3e-0ead-498f-b2cb-3531ea0788ef</UUID>\r\n" +
            "      </TiffData>\r\n" +
            "      <Plane TheZ=\"0\" TheT=\"0\" TheC=\"0\" />\r\n" +
            "      <Plane TheZ=\"0\" TheT=\"0\" TheC=\"1\" />\r\n" +
            "    </Pixels>\r\n" +
            "  </Image>\r\n" +
            "</OME>";

        [TestMethod]
        public void ReadTest()
        {
            XmlSerializer Serializer = new XmlSerializer((typeof(OME)));
            OME metaData;
            using (TextReader tr = new StringReader(META))
            {
                metaData = (OME)Serializer.Deserialize(tr);
            }

            Assert.IsNotNull(metaData, "Metadata is null");
            Assert.AreEqual(metaData.Creator, "OME Bio-Formats 5.2.0-m4", "Creator is not equal");
            Assert.AreEqual(metaData.Items.Length, 4, "Incorrect number of items");
        }

        [TestMethod]
        public void WriteTest()
        {
            XmlSerializer Serializer = new XmlSerializer((typeof(OME)));
            OME metaData;
            using (TextReader tr = new StringReader(META))
            {
                metaData = (OME)Serializer.Deserialize(tr);
            }

            string omeMeta = Serialize(metaData);
            Assert.IsTrue(omeMeta.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Incorrect heading");
            Assert.AreEqual(omeMeta, META);
        }

        /// <summary>
        /// Serializes an object to a utf-8 xml string.
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="toSerial">The object to serialize</param>
        /// <returns>A UTF-8 encoded string.</returns>
        public static string Serialize<T>(T toSerial)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter sw = new Utf8StringWriter())
            {
                serializer.Serialize(sw, toSerial);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Custom Stringwriter to force UTF-8 encoding.
        /// </summary>
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

    }
}
