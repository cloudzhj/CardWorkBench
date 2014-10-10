using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CardWorkbench.Models
{
    public class FrameModel 
    {
        public int Number { get; set; }

        public string SyncWord { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Word3 { get; set; }
        public string ID { get; set; }
        public string Word5 { get; set; }
        public string Word6 { get; set; }
        public string Word7 { get; set; }
        public string Word8 { get; set; }
        public string Word9 { get; set; }
        public string Word10 { get; set; }
        public string Word11 { get; set; }
        public string Word12 { get; set; }

        public FrameModel(int frameID, string pcm1, string pcm2, string pcm3, string pcm4, string pcm5, string pcm6, string pcm7, string pcm8, string pcm9, string pcm10, string pcm11, string pcm12)
        {
            this.Number = frameID;
            this.SyncWord = "ABBC";
            this.Word1 = pcm1;
            this.Word2 = pcm2;
            this.Word3 = pcm3;
            this.ID = pcm4;
            this.Word5 = pcm5;
            this.Word6 = pcm6;
            this.Word7 = pcm7;
            this.Word8 = pcm8;
            this.Word9 = pcm9;
            this.Word10 = pcm10;
            this.Word11 = pcm11;
            this.Word12 = pcm12;
        }
    }
}
