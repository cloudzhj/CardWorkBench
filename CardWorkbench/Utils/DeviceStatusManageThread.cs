using CardWorkbench.Models;
using CardWorkbench.ViewModels;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.NavBar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CardWorkbench.Utils
{
    /// <summary>
    /// 设备状态管理线程类
    /// </summary>
    public class DeviceStatusManageThread
    {
        private volatile bool isTimerPause = false;
        private DispatcherTimer timer = new DispatcherTimer();
        private Acro.ACRO1626P acro1626P = new Acro.ACRO1626P();
        private NavBarControl menuNavBarControl;
        private static DeviceStatusManageThread deviceStatusManageThread;

        public static void initDeviceStatusManageThread(NavBarControl menuNavBarControl){
            if (deviceStatusManageThread == null)
	        {
		       deviceStatusManageThread = new DeviceStatusManageThread(menuNavBarControl);
	        }
        }


        private DeviceStatusManageThread(NavBarControl menuNavBarControl)
            {
                this.menuNavBarControl = menuNavBarControl;

                //间隔时间设置
                timer.Interval = TimeSpan.FromMilliseconds(2000);
                if (!isTimerPause)
                {
                    timer.Tick += new EventHandler(DoWork);
                }
                timer.Start();
            }

            public void DoWork(object sender, EventArgs e)
            {
                IList<Device> listDevice = DevicesManager.getCurrentDeviceListInstance();
                if (listDevice != null)
                {
                    //bool isStatusChanged = false; //状态是否变化标志
                    foreach (Device device in listDevice)
                    {
                        //查询并更新通道状态
                        if (device.channelList != null)
                        {
                            foreach (Channel channel in device.channelList)
                            {
                                //1.TODO 访问板卡提供的接口，获得最新的通道状态信息
                                ChannelStatus channelStatus = null;
                                try
                                {
                                    string statusJson = acro1626P.getChannelStatus(int.Parse(device.deviceID), int.Parse(channel.channelID));
                                    var str = JObject.Parse(statusJson).SelectToken("ChannelStatus").ToString();
                                    channelStatus = JsonConvert.DeserializeObject<ChannelStatus>(str);
                                }
                                catch (Exception ex)
                                {
                                    //可以考虑输出到日志panel
                                    //throw ex;
                                    Console.WriteLine(ex.Message);
                                }

                                //////2.TODO 更新通道状态变化到ui
                                bool isSuccessUpdated = updateChannelStatus(channelStatus, channel);
                                if (isSuccessUpdated)
                                {
                                    updateChannelNavBarItemStatusUI(menuNavBarControl, device, channel);
                                }
                                //////
                            }
                        }
                        //查询并更新模拟器状态
                        if (device.simulator != null)
                        {
                            //访问板卡提供的接口,获得最新的模拟器状态信息
                            SimulatorStatus simulatorStatus = null;
                            try
                            {
                                string statusJson = acro1626P.getSimulatorStatus(int.Parse(device.deviceID));
                                var str = JObject.Parse(statusJson).SelectToken("SimulatorStatus").ToString();
                                simulatorStatus = JsonConvert.DeserializeObject<SimulatorStatus>(str);
                            }
                            catch (Exception ex)
                            {
                               //可以考虑输出到日志panel
                               //throw ex;
                               Console.WriteLine(ex.Message);
                            }

                            //////2.TODO 更新模拟器状态变化到ui
                            bool isSuccessUpdated = updateSimulatorStatus(simulatorStatus, device.simulator);
                            if (isSuccessUpdated)
                            {
                                updateSimulatorNavBarItemStatusUI(menuNavBarControl, device.simulator);
                            }
                        }
                    }
                    //if (isStatusChanged)
                    //{
                    //}
                    DevicesManager.setCurrentDeviceList(listDevice);

                }
                Console.WriteLine("hahahahahahahahahahahahahahahahaha");
            }

            private bool updateChannelStatus(ChannelStatus newChannelStatus, Channel currentChannel)
            {
                if (newChannelStatus != null)   //如果获取的对象不为空，则更新现有状态
                {
                    currentChannel.channelStatus = newChannelStatus;
                    return true;
                }
                return false;
            }

            private void updateChannelNavBarItemStatusUI(NavBarControl menuNavBarControl, Device device, Channel channel)
            {
                string stackPanelName = MainWindowViewModel.NAVBARITEM_CHANNEL_NAME_PREFIX + device.deviceID + channel.channelID + MainWindowViewModel.NAVBARITEM_STACKPANEL_NAME_SUBFIX;
                string lightStatusImageUri = channel.channelStatus.bRun == MainWindowViewModel.CHANNELSTATUS_BRUN_ON ? MainWindowViewModel.NAVBARITEM_CHANNEL_ON_URI_PATH : MainWindowViewModel.NAVBARITEM_CHANNEL_OFF_URI_PATH;
                string statusLabelContent = channel.channelName + (channel.channelStatus.bRun == MainWindowViewModel.CHANNELSTATUS_BRUN_ON ? MainWindowViewModel.LABEL_NAVBARITEM_ON : MainWindowViewModel.LABEL_NAVBARITEM_OFF);
                updateNavBarItemStackPanelUI(stackPanelName, lightStatusImageUri, statusLabelContent);
            }

            private bool updateSimulatorStatus(SimulatorStatus newSimulatorStatus, Simulator currentSimulator)
            {
                if (newSimulatorStatus != null)   //如果获取的对象不为空，则更新现有状态
                {
                    currentSimulator.simulatorStatus = newSimulatorStatus;
                    return true;
                }
                return false;
            }

            private void updateSimulatorNavBarItemStatusUI(NavBarControl menuNavBarControl, Simulator simulator)
            {
                string stackPanelName = MainWindowViewModel.NAVBARITEM_SIMULATOR_NAME_PREFIX + simulator.simulatorID + MainWindowViewModel.NAVBARITEM_STACKPANEL_NAME_SUBFIX;
                string lightStatusImageUri = simulator.simulatorStatus.SIMULATOR_STATUS != SimulatorStatus.SIMULATORSTATUS.IDLE ? MainWindowViewModel.NAVBARITEM_CHANNEL_ON_URI_PATH : MainWindowViewModel.NAVBARITEM_CHANNEL_OFF_URI_PATH;
                string statusLabelContent = simulator.simulatorName + 
                    (simulator.simulatorStatus.SIMULATOR_STATUS == SimulatorStatus.SIMULATORSTATUS.RUNA ? 
                    MainWindowViewModel.LABEL_NAVBARITEM_SIMULATOR_RUNA : 
                    simulator.simulatorStatus.SIMULATOR_STATUS == SimulatorStatus.SIMULATORSTATUS.RUNB ?
                    MainWindowViewModel.LABEL_NAVBARITEM_SIMULATOR_RUNB : 
                    simulator.simulatorStatus.SIMULATOR_STATUS == SimulatorStatus.SIMULATORSTATUS.INSERTFRAME ?
                    MainWindowViewModel.LABEL_NAVBARITEM_SIMULATOR_INSERTFRAME : 
                    MainWindowViewModel.LABEL_NAVBARITEM_OFF);
                updateNavBarItemStackPanelUI(stackPanelName, lightStatusImageUri, statusLabelContent);
            }

            private void updateNavBarItemStackPanelUI(string stackPanelName, string statusLightImageUri, string statusLabelContent) 
            { 
                if (menuNavBarControl != null)
                {
                    menuNavBarControl.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        StackPanel simulatorNavBarStackPanel = LayoutHelper.FindElementByName(menuNavBarControl, stackPanelName) as StackPanel;

                        foreach (var item in simulatorNavBarStackPanel.Children)
                        {
                            if (item.GetType().Equals(typeof(Image)))
                            {
                                Image itemImage = item as Image;
                                BitmapImage imageSource = new BitmapImage(new Uri(statusLightImageUri));
                                itemImage.Source = imageSource;
                            }
                            else if (item.GetType().Equals(typeof(Label)))
                            {
                                Label itemLabel = item as Label;
                                itemLabel.Content = statusLabelContent;
                            }
                        }
                    }));

                }
            }

            public void RequestStop()
            {
                isTimerPause = true;
            }

    }
}
