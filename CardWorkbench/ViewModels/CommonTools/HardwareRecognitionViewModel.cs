using CardWorkbench.Models;
using CardWorkbench.Utils;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace CardWorkbench.ViewModels.CommonTools
{
    public class HardwareRecognitionViewModel
    {
        public readonly string GRID_HARDWARERECOGNITION_NAME = "hardwareRecognitionGrid";   //硬件设备grid名称

        public ICommand hardwareRecognitionLoadedCommand
        {
            get { return new DelegateCommand<RoutedEventArgs>(onHardwareRecognitionLoaded, x => { return true; }); }
        }

        private void onHardwareRecognitionLoaded(RoutedEventArgs e)
        {
            FrameworkElement root = LayoutHelper.GetTopLevelVisual(e.Source as DependencyObject);
            GridControl grid = LayoutHelper.FindElementByName(root, GRID_HARDWARERECOGNITION_NAME) as GridControl;
            grid.ItemsSource = findDevice();
        }

        private static IList<Device> findDevice()
        {
            List<Device> hardwareList = new List<Device>();
            try
            {
                XmlDocument testXmlDoc = XmlParserUtil.getXmlDocumnetInstance("C:\\Users\\Neptune\\Desktop\\HardWare.xml");
                //XmlNode xmlNode = XmlParserUtil.getXmlSingleNodeByName("Module");
                //Console.WriteLine(XmlParserUtil.getNodeAttributeValue(xmlNode, "Name"));
                XmlNodeList nodeList = XmlParserUtil.getChildNodesListByParentNodeName("Box");
                foreach (XmlNode deviceNode in nodeList)
                {
                    Device device = new Device();
                    string deviceID = XmlParserUtil.getNodeAttributeValue(deviceNode, "id");
                    string deviceModel = XmlParserUtil.getNodeAttributeValue(deviceNode, "model");
                    device.deviceID = int.Parse(deviceID);
                    device.deviceModel = deviceModel;
                    hardwareList.Add(device);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return hardwareList;
        }

    }
}
