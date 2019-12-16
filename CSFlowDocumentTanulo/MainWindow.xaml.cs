
using CSFlowDocumentTanulo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace CSFlowDocumentTry1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, Tanulo> TanuloDictionary = new Dictionary<string, Tanulo>();
        public TanuloKontener tanuloKontener = new TanuloKontener()
        {
            TanuloLista = new List<Tanulo>()
        };
        public int i = 1;

        public MainWindow()
        {
            InitializeComponent();

           // uiListViewFlowDocument.Items.Add(uiListViewItem);
           // uiListViewFlowDocument.Items.Add(uiListViewItem);
        }

        private void AddTextBox(object subject, RoutedEventArgs e)
        { 
            WrapPanel uiMainWrapPanel = new WrapPanel() {
                Orientation = Orientation.Vertical,
                Width = 750,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            WrapPanel uiInnerWrapPanel1 = new WrapPanel() {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                }
            };

            uiInnerWrapPanel1.Children.Add(
                new Label() {
                    Content = "Vezetéknév: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxVezetekNev",
                    Width = 250,
                    Height = 25
                });

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Keresztnév: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxKeresztNev",
                    Width = 250,
                    Height = 25
                });

            WrapPanel uiInnerWrapPanel2 = new WrapPanel() {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                }
            };

            uiInnerWrapPanel2.Children.Add(
                new Label()
                {
                    Content = "Kor: ",
                    Width = 100
                });

            uiInnerWrapPanel2.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxKor",
                    Width = 250,
                    Height = 25
                });

            uiInnerWrapPanel2.Children.Add(
                new Label()
                {
                    Content = "Cím: ",
                    Width = 100
                });

            uiInnerWrapPanel2.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxCim",
                    Width = 250,
                    Height = 25
                });
            BlockUIContainer uiContainer = new BlockUIContainer()
            {
                Child = uiMainWrapPanel,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0,0,0)),
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 1,
                    Top = 1,
                    Right = 1
                }
                
            };
            Section sectionFromCode = new Section()
            {
                Background = new SolidColorBrush(Color.FromRgb(248, 248, 255)),
                Name = "section" + i
            };

            Button uiTorlesButton = new Button()
            {
                Width = 200,
                Content = "Törlés",
                Tag = sectionFromCode.Name
            };

            uiTorlesButton.Click += deleteButton;

            uiMainWrapPanel.Children.Add(uiInnerWrapPanel1);
            uiMainWrapPanel.Children.Add(uiInnerWrapPanel2);
            uiMainWrapPanel.Children.Add(uiTorlesButton);


            sectionFromCode.Blocks.Add(uiContainer);
            uiFlowDocument.Blocks.Add(sectionFromCode);

            
            TanuloDictionary.Add(sectionFromCode.Name, new Tanulo());

            i++;
        }

        private void ButtonAction(object subject, RoutedEventArgs e)
        {
            tanuloKontener.TanuloLista.Clear();
            
            Tanulo tanulo = new Tanulo()
            {
                Vezeteknev = uiTextBoxVezetekNev.Text.ToString(),
                Keresztnev = uiTextBoxKeresztNev.Text.ToString(),
                Kor = uiTextBoxKor.Text.ToString(),
                Cim = uiTextBoxCim.Text.ToString()
            };

            tanuloKontener.TanuloLista.Add(tanulo);
            
            foreach (var block in uiFlowDocument.Blocks)
            {
                foreach (var dictionary in TanuloDictionary)
                {
                    if (block.Name.Equals(dictionary.Key))
                    {
                        Section sectionBlock = (Section)block;
                        BlockUIContainer uiContainmer = (BlockUIContainer)sectionBlock.Blocks.FirstBlock;
                        WrapPanel uiMainWrapPanel = (WrapPanel)uiContainmer.Child;
                        UIElementCollection uiInnerWrapPanels = uiMainWrapPanel.Children;
                        foreach(var uiInnerWrapPanel in uiInnerWrapPanels)
                        {
                            UIElementCollection uiWrapPanelElements = uiMainWrapPanel.Children;
                            foreach(var element in uiWrapPanelElements)
                            {
                                if (element.GetType().Name.Equals("WrapPanel"))
                                {
                                    WrapPanel wrapPanel = (WrapPanel)element;
                                    foreach(var elem in wrapPanel.Children)
                                    {
                                        if (elem.GetType().Name.Equals("TextBox"))
                                        {
                                            TextBox uiTextBox = (TextBox)elem;
                                            if (uiTextBox.Name.Equals("uiTextBoxVezetekNev"))
                                            {
                                                dictionary.Value.Vezeteknev = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxKeresztNev"))
                                            {
                                                dictionary.Value.Keresztnev = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxKor"))
                                            {
                                                dictionary.Value.Kor = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxCim"))
                                            {
                                                dictionary.Value.Cim = uiTextBox.Text;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var dictionary in TanuloDictionary)
            {
                tanuloKontener.TanuloLista.Add(dictionary.Value);
            }

            uiTextBlock.Text = serialize(tanuloKontener, typeof(TanuloKontener));
            /*
            uiTextBoxName.Text = "";
            uiTextBoxAncestor.Text = "";
            uiTextBoxGUID.Text = "";
            uiTextBoxNamespace.Text = "";
            */
        }


        public string serialize(Object taroltEljaras, Type anyType)
        {
            XmlSerializer serializer = new XmlSerializer(anyType);

            var xml = "";
            
            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    serializer.Serialize(writer, taroltEljaras);
                    xml = writer.ToString(); // Your XML
                }
            }
            //System.IO.File.WriteAllText(path, xml);
            
            return xml;

        }

        private void deleteButton(object subject, RoutedEventArgs e)
        {
            var clickedButton = subject as Button;
            string sectionName = clickedButton.Tag.ToString();

            List<Block> flowDocumentBlockList = uiFlowDocument.Blocks.ToList();
            uiFlowDocument.Blocks.Clear();

            foreach(var block in flowDocumentBlockList)
            {
                if (!block.Name.Equals(sectionName))
                {
                    uiFlowDocument.Blocks.Add(block);
                }
            }

            TanuloDictionary.Remove(sectionName);
            tanuloKontener.TanuloLista.Clear();
            
            
        }

        public class Person
        {
            public string name { get; set; }
            public string age { get; set; }
        }
    }
}
