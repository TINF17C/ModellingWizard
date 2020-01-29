using Aml.Editor.Plugin.Contracts;
using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Aml.Editor.Plugin
{
    /// <summary>
    /// ModellingWizard is an PlugIn, which implements the IAMLEditorView Interface. The PlugIn is
    /// a UserControl, which is managed by the AutomationML Editors Window- and Docking - Manager.
    /// The Export Attribute enables the AutomationML Editor to load the PlugIn with the <a
    /// href="http://msdn.microsoft.com/en-us/library/dd460648%28v=vs.110%29.aspx">Microsoft Managed
    /// Extensibility Framework</a>.
    /// </summary>
    [Export(typeof(IAMLEditorView))]
    public partial class ModellingWizard : UserControl, IAMLEditorView
    {
        /// <summary>
        /// <see cref="AboutCommand"/>
        /// </summary>
        private RelayCommand<object> aboutCommand;

        MWController mWController;

        public ModellingWizard()
        {
            mWController = new MWController(this);
            
            // Defines the Command list, which will contain user commands, which a user can select
            // via the PlugIn Menu.
            Commands = new List<PluginCommand>();


            // Every PlugIn needs at least an Activation command, which will be called by a user to activate the PlugIn.
            ActivatePlugin = new PluginCommand()
            {
                Command = new RelayCommand<object>(this.StartCommandExecute, 
                    this.StartCommandCanExecute),
                CommandName = "Start",
                CommandToolTip = "Start the PlugIn"
            };

            // Every PlugIn should provide a Termination command, which will be called when the PlugIn window is closed by the user. This can only
            // occur, if the PlugIn view is embedded in a docking window by the Editor.
            TerminatePlugin = new PluginCommand()
            {
                Command = new RelayCommand<object>(this.StopCommandExecute, this.StopCommandCanExecute),
                CommandName = "Stop",
                CommandToolTip = "Stop the PlugIn"
            };

            ReloadObjects = new PluginCommand()
            {
                Command = new RelayCommand<object>(this.ReloadObjectsExecute, this.StopCommandCanExecute),
                CommandName = "Reload Objects",
                CommandToolTip = "Reload all .amlx in the plugin folder"
            };

            InitializeComponent();


            // Add the StartCommand (should exist in any PlugIn)
            Commands.Add(ActivatePlugin);

            // Add the Stop Command (should exist in any PlugIn)
            Commands.Add(TerminatePlugin);

            Commands.Add(ReloadObjects);

            //// Add the change spelling command (an additional command, only for this special PlugIn)
            //Commands.Add(new PluginCommand()
            //{
            //    CommandName = "Load File",
            //    Command = InvertCase,
            //    CommandToolTip = "Load a new test file"
            //});

            // Add the About Command (recommended to exist in any PlugIn)
            Commands.Add(new PluginCommand()
            {
                CommandName = "About",
                Command = AboutCommand,
                CommandToolTip = "Information about this PlugIn"
            });

            this.IsActive = false;
        }

        private void ReloadObjectsExecute(object obj)
        {
           
        }

        /// <summary>
        /// Occurs when the PlugIn is activated (for example via the <see cref="StartCommand"/> ).
        /// </summary>
        public event EventHandler PluginActivated;

        /// <summary>
        /// Occurs when the PlugIn is deactivated (some UserInteraction inside the PlugIn or via the
        /// <see cref="StopCommand"/> ).
        /// </summary>
        public event EventHandler PluginTerminated;
        
        /// <summary>
        /// The AboutCommand - Command
        /// </summary>
        /// <value>The about command.</value>
        public System.Windows.Input.ICommand AboutCommand
        {
            get
            {
                return this.aboutCommand
                ??
                (this.aboutCommand = new RelayCommand<object>(this.AboutCommandExecute, this.AboutCommandCanExecute));
            }
        }

        /// <summary>
        /// Gets the Command to activate the PlugIn.
        /// </summary>
        public PluginCommand ActivatePlugin
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Command to reload the AMLX Files in ./modellingwizard/
        /// </summary>
        public PluginCommand ReloadObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this UserControl could be closed from the Editor's
        /// WindowManager. When a close occurs from the WindowManager, the StopCommand will be
        /// executed via the <see cref="ExecuteCommand"/> Method.
        /// </summary>
        /// <value><c>true</c> if this instance can close; otherwise, <c>false</c>.</value>
        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the List of commands, which are viewed in the PlugIn Menu in the Host Application
        /// </summary>
        /// <value>The command List.</value>
        public List<PluginCommand> Commands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the display name which is shown in the PlugIn Menu in the Host Application
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return "Modeling Wizard for Devices (V 1.0.0.1)(21.10.2019)"; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is active. The Property should be set to
        /// true in the <see cref="StartCommand"/> and set to false in the <see cref="StopCommand"/>
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is reactive. Reactive PlugIn will be
        /// notified, when the actual CAEX-Object changes (Selection of the Tree view Item) <see
        /// cref="ChangeAMLFilePath"/> and <see cref="ChangeSelectedObject"/>.
        /// </summary>
        /// <value><c>true</c> if this instance is reactive; otherwise, <c>false</c>.</value>
        public bool IsReactive
        {
            get { return false; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is read only. A Read only PlugIn should not
        /// change any CAEX Objects.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadonly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the terminate PlugIn command.
        /// </summary>
        public PluginCommand TerminatePlugin
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the initial dock position for the PlugIn window.
        /// </summary>
        public DockPositionEnum InitialDockPosition => DockPositionEnum.Floating;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic active when loaded.
        /// This value can be initially set and will be defined by the user.
        /// </summary>
        public bool IsAutoActive { get; set; }

        /// <summary>
        /// Gets the package name which is used to download the PlugIn package from a NuGet feed. If a Package name
        /// is defined, the AMLEditor can update PlugIn packages independently from its own update cycle.
        /// </summary>
        /// <value>
        /// The package name.
        /// </value>
        public string PackageName => "";

        /// <summary>
        /// Gets the image which should be used in the Header of the PlugIn window. 
        /// If no image is defined the editor uses a default image.
        /// </summary>
        public BitmapImage PaneImage => null;


        /// <summary>
        /// Changes the current amlFilePath. The Host Application will call this method when the
        /// PlugIns <see cref="P:Aml.Editor.Plugin.Contracts.IAMLEditorPlugin.IsReactive" /> Property is set to true and the Currently opened
        /// AutomationML File changes in the AMLEditor Host Application.
        /// </summary>
        /// <param name="amlFilePath">The Path to the current AML File in the AML Editor.</param>
        public void ChangeAMLFilePath(string amlFilePath)
        {
            //mWController.ChangeAMLFilePath(amlFilePath);
        }


        /// <summary>
        /// Changes the selected object. The Host Application will call this method when the PlugIns
        /// <see cref="P:Aml.Editor.Plugin.Contracts.IAMLEditorPlugin.IsReactive" /> Property is set to true and the Current Selection changes in
        /// the AMLEditor Host Application.
        /// </summary>
        /// <param name="selectedObject">The selected CAEX - object.</param>
        public void ChangeSelectedObject(CAEXBasicObject selectedObject)
        {
            /*if (selectedObject is InternalElementType ie)
            {
                if (ie.SystemUnitClass != null)
                {
                    PlugInUI.ShowClass(ie.SystemUnitClass);
                }
                
            }*/
        }

        /// <summary>
        /// This Method is called from the AutomationML Editor to execute a specific command. The
        /// Editor can only execute those commands, which are identified by the <see
        /// cref="PluginCommandsEnum"/> Enumeration. The Editor may execute the termination command
        /// of the PlugIn, so here some preparations for a clean termination should be performed.
        /// </summary>
        /// <param name="command">    The command.</param>
        /// <param name="amlFilePath">The amlFilePath.</param>
        public void ExecuteCommand(PluginCommandsEnum command, string amlFilePath)
        {
            switch (command)
            {
                case PluginCommandsEnum.Terminate:
                    StopCommandExecute(null);
                    break;
            }
        }

        /// <summary>
        /// This Method is called on activation of a PlugIn. The AutomationML Editor 'publishes' its
        /// current state to the PlugIn, that is the Path of the loaded AutomationML Document and
        /// the currently selected AutomationML Object'. Please note, that the objects may be empty
        /// or null.
        /// </summary>
        /// <param name="amlFilePath">   The AML file path, may be empty.</param>
        /// <param name="selectedObject">The selected object, may be null.</param>
        public void PublishAutomationMLFileAndObject(string amlFilePath, CAEXBasicObject selectedObject)
        {
            
        }

        /// <summary>
        /// Test, if the <see cref="AboutCommand"/> can execute.
        /// </summary>
        /// <param name="parameter">unused.</param>
        /// <returns>true, if command can execute</returns>
        private bool AboutCommandCanExecute(object parameter)
        {
            // Execution is always possible, also for inactive PlugIns
            return true;
        }

        /// <summary>
        /// The <see cref="AboutCommand"/> Execution Action.
        /// </summary>
        /// <param name="parameter">unused.</param>
        private void AboutCommandExecute(object parameter)
        {
            var dialog = new About();
            dialog.ShowDialog();
        }

       
       

        /// <summary>
        /// Test, if the <see cref="StartCommand"/> can execute. The <see cref="IsActive"/> Property
        /// should be false prior to Activation.
        /// </summary>
        /// <param name="parameter">unused</param>
        /// <returns>true, if command can execute</returns>
        private bool StartCommandCanExecute(object parameter)
        {
            return !this.IsActive;
        }

        /// <summary>
        /// The <see cref="StartCommand"/> s execution Action. The <see cref="PluginActivated"/>
        /// event is raised and the <see cref="IsActive"/> Property is set to true.
        /// </summary>
        /// <param name="parameter">unused</param>
        private void StartCommandExecute(object parameter)
        {
            this.IsActive = true;
            
            // create the new PlugInUI and add it to the forms host
            mWController.ChangeGui(MWController.MWGUIType.DeviceDescription);

            PluginActivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Test, if the <see cref="StopCommand"/> can execute.
        /// </summary>
        /// <param name="parameter">unused</param>
        /// <returns>true, if command can execute</returns>
        private bool StopCommandCanExecute(object parameter)
        {
            return this.IsActive;
        }

        /// <summary>
        /// The <see cref="StopCommand"/> Execution Action sets the <see cref="IsActive"/> Property
        /// to false. The <see cref="PluginTerminated"/> event will be raised.
        /// </summary>
        /// <param name="parameter">unused</param>
        private void StopCommandExecute(object parameter)
        {
            this.IsActive = false;
            PluginTerminated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Replace the Content in the Plugin window with the Windows Forms GUI
        /// </summary>
        /// <param name="content">The Form to be displayed in the plugin window</param>
        public void changeGUI(System.Windows.Forms.Control content)
        {
            FormsHost.Child = content;
        }
    }
}