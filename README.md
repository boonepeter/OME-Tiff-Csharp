# OME-Tiff-Csharp

Helper class for serializing image metadata into Open Microscopy's OME-XML Tiff format.

This class is auto generated from the most recent [OME-TIFF schema](https://www.openmicroscopy.org/Schemas/OME/index.html).

The schema used can also be found in [Schema](Schema/).

Here is an example using [Bitmiracles's LibTiff.NET](https://bitmiracle.com/libtiff/) to open the image:

```csharp
using System.IO;
using System.Xml.Serialization;
using BitMiracle.LibTiff.Classic;
using OMETiffCsharp.OMETiff;

public void ReadMetadata(string filename)
{
    OME Metadata;
    // Open tiff file using LibTiff.NET
    using (Tiff input = Tiff.Open(filename, "r"))
    {
        // Get OME metadata from description tag
        string desc = input.GetField(TiffTag.IMAGEDESCRIPTION)[0].ToString();
        
        XmlSerializer Serializer = new XmlSerializer((typeof(OME)));
        using (TextReader tr = new StringReader(desc))
        {
            Metadata = (OME)Serializer.Deserialize(tr);
        }
    }
    
    // Now access the elements. 
    // Can iterate through items
    string creator = Metadata.Creator;
    var items = Metadata.Items;
    for (int i = 0; i < items.Length; i++)
    {
        if (items[i].GetType() == typeof(Image))
        {
            // process Image type  
	}
}
```

You can look at the [tests](./OMETest) for working examples.

#  

This work is derived in part from the OME specification. Copyright (C) 2002-2016 Open Microscopy Environment
