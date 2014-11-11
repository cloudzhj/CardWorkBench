using CardWorkbench.Models;
using CardWorkbench.ViewModels;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.NavBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CardWorkbench.Utils
{
    public class DeviceStatusManageThread
    {
        private DispatcherTimer timer = new DispatcherTimer();
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
                //主动查询并接收状态更新json
                IList<Device> listDevice = DevicesManager.getCurrentDeviceListInstance();
                if (listDevice != null)
                {
                    bool isStatusChanged = false; //状态是否变化标志
                    foreach (Device device in listDevice)
                    {
                        if (device.channelList != null)
                        {
                            foreach (Channel channel in device.channelList)
                            {
                                //1.TODO 访问板卡提供的接口，获得最新的设备状态信息
                                //string statusJson = searchLatestStatusByDeviceIdAndChannelId(device.deviceID, channel.channelID);
                                //ChannelStatus channelStatus = Convert(statusJson)
                                
                                //////2.TODO 比较通道状态是否有变化，有变化更新到全局变量
                                if ("2".Equals(channel.channelID))
                                {
                                    if (channel.channelStatus == null)  //测试第一次更新状态
                                    {
                                      ChannelStatus channelStatus = new ChannelStatus();
                                      channelStatus.bRun = 1;
                                      channel.channelStatus = channelStatus;
                                      isStatusChanged = true;
                                      //3.TODO 更新到界面
                                        if (menuNavBarControl != null)
                                        {
                                            menuNavBarControl.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                StackPanel channelNavBarStackPanel = LayoutHelper.FindElementByName(menuNavBarControl, MainWindowViewModel.NAVBARITEM_CHANNEL_NAME_PREFIX + device.deviceID + channel.channelID + MainWindowViewModel.NAVBARITEM_STACKPANEL_NAME_SUBFIX) as StackPanel;
                                                foreach (var item in channelNavBarStackPanel.Children)
                                                {
                                                    if (item.GetType().Equals(typeof(Image)))
                                                    {
                                                        Image itemImage = item as Image;
                                                        BitmapImage imageSource = new BitmapImage(new Uri(channel.channelStatus.bRun == 1 ? MainWindowViewModel.NAVBARITEM_CHANNEL_ON_URI_PATH : MainWindowViewModel.NAVBARITEM_CHANNEL_OFF_URI_PATH));
                                                        itemImage.Source = imageSource;
                                                    }
                                                    else if (item.GetType().Equals(typeof(Label)))
                                                    {
                                                        Label itemLabel = item as Label;
                                                        itemLabel.Content = channel.channelName + (channel.channelStatus.bRun == 1 ? MainWindowViewModel.LABEL_NAVBARITEM_ON : MainWindowViewModel.LABEL_NAVBARITEM_OFF);
                                                    }
                                                }
                                            }));

                                        }
                                    }
                                    
                                }
                                //////
                            }
                        }
                    }
                    if (isStatusChanged)
                    {
                        DevicesManager.setCurrentDeviceList(listDevice);
                    
                    }

                }
                Console.WriteLine("hahahahahahahahahahahahahahahahaha");
            }
            public void RequestStop()
            {
                isTimerPause = true;
            }
            // Volatile is used as hint to the compiler that this data
            // member will be accessed by multiple threads.
            private volatile bool isTimerPause = false;
    }
}
