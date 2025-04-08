using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlanMyNight.PlanMyNightDockables {
    [Export(typeof(ResourceDictionary))]
    public partial class PlanMyNightDockableTemplates : ResourceDictionary {
        public PlanMyNightDockableTemplates() {
            InitializeComponent();
        }
    }
}