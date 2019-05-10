# ModellingWizardPlugin
This is a Plugin for the AutomationML Editor

# Setup Projekt for development
1. Open the .sln in Visual Studio
2. Make sure that the NuGet Packages are installed (Check using: 'Extras' -> 'NuGet-Packet-Manager' -> 'NuGet-Packete für diese Projektmappe verwalten')
3. Install the Plugin Sandbox by cloning https://github.com/AutomationML/AMLEditorPluginContract
4. Open the AMLEditorPluginContract/Templates/AmlEditorPlugInSandbox/AmlEditorPlugInSandbox.sln

# Build and Run
1. Build the AmlEditorPlugInSandbox Project
2. Build the ModellingWizard
3. Copy the ModellingWizard Folder from /Plugins to /AmlEditorPlugInSandbox/bin/Debug/PlugIns
4. Run AmlEditorPlugInSandbox.exe in /AmlEditorPlugInSandbox/bin/Debug/