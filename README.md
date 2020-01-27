# ModellingWizardPlugin
This is a Plugin for the AutomationML Editor.
It is able to create new AML Devices and Interfaces and save them as AMLX Package.
It can also import IODD and GSDML Files using Converters

This project was developed as a student project at the [Cooperative State University (DHBW)](https://dhbw-stuttgart.de) in Stuttgart under supervision of [Markus Rentschler](http://wwwlehre.dhbw-stuttgart.de/~rentschler/) by the following student team in 2018/2019:
* Burkowitz, Steffen
* Löffler, Tobias
* Mayer, Simon
* Joukhadar, Abdulkarim
* Wandel, Simon

This project was further developed as a Mater Thesis work at [Innovative Software Services GmbH Stuttgart] by student from [Otto-Von-Guericke University Magdeburg] under supervision of Markus Rentschler.
* Raj Kumar Pulaparthi

## Missing Labels and Text
Due to a bug in the window manager of the AMLEditor if you are using the theme "Metro Light" the text and icon will not be visible on startup.
To fix this, simply select another theme using "View" > "Change Theme".
(See also this [issue](https://github.com/TINF17C/ModellingWizard/issues/9))

# AML Devices Format
The Plugin creates Devices with the following DeviceIdentification InternalElement:
```xml
<InternalElement Name="DeviceIdentification" ID="%DEVICEID%">
    <Attribute Name="CommunicationTechnology" AttributeDataType="xs:string" />
    <Attribute Name="VendorName" AttributeDataType="xs:string">
    <Attribute Name="DeviceName" AttributeDataType="xs:string"/>
    <Attribute Name="DeviceFamiliy" AttributeDataType="xs:string"/>
    <Attribute Name="ProductName" AttributeDataType="xs:string"/>
    <Attribute Name="OrderNumber" AttributeDataType="xs:string"/>
    <Attribute Name="ProductText" AttributeDataType="xs:string" />
    <Attribute Name="IPProtection" AttributeDataType="xs:string" />
    <Attribute Name="VendorHompage" AttributeDataType="xs:string" />
    <Attribute Name="HardwareRelease" AttributeDataType="xs:string" />
    <Attribute Name="SoftwareRelease" AttributeDataType="xs:string" />
    <Attribute Name="OperatingTemperatureMin" AttributeDataType="xs:double"/>
    <Attribute Name="OperatingTemperatureMax" AttributeDataType="xs:double"/>
    <Attribute Name="VendorId" AttributeDataType="xs:integer"/>
    <Attribute Name="DeviceId" AttributeDataType="xs:integer"/>
</InternalElement>
```

If a Device has this InternelElement Attributes, our Plugin will be able to display and modifiy the device.

# Setup Projekt for development
1. Open the .sln in Visual Studio
2. Make sure that the NuGet Packages are installed (Check using: 'Extras' -> 'NuGet-Packet-Manager' -> 'NuGet-Packete für diese Projektmappe verwalten')
3. Install the AMLEditor Version 5.1.3 or newer
4. To test the plugin, copy the build output (ModellingWizard.dll) to the PlugIn folder of the AMLEditor



# Honorable mention
Christian K. und Philipp A.
