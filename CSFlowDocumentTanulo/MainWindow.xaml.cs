
using CSAc4yObjectObjectService.Association;
using CSAc4yObjectObjectService.Object;
using CSFlowDocumentTanulo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

        public string APPSETTING_SQLCONNECTIONSTRING = ConfigurationManager.AppSettings["sqlConnectionString"];
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
            AddForm("", "", "", "");
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

            UploadTanuloKontener(tanuloKontener);
            UploadKontenerTanuloAssociation(tanuloKontener);

            /*
            uiTextBoxName.Text = "";
            uiTextBoxAncestor.Text = "";
            uiTextBoxGUID.Text = "";
            uiTextBoxNamespace.Text = "";
            */
        }
    
        public void XmlBetoltes(object subject, RoutedEventArgs e)
        {
            List<Block> flowDocumentBlockList = uiFlowDocument.Blocks.ToList();
            uiFlowDocument.Blocks.Clear();

            foreach (var block in flowDocumentBlockList)
            {
                if (block.Name.Equals("section0"))
                {
                    uiFlowDocument.Blocks.Add(block);
                }
            }

            TanuloDictionary.Clear();
            tanuloKontener.TanuloLista.Clear();

            string xml = uiTextBlock.Text;
            TanuloKontener kontenerUj = deserialize(xml);
            int x = 0;

            foreach(var tanulo in kontenerUj.TanuloLista)
            {
                if (x == 0)
                {
                    uiTextBoxCim.Text = tanulo.Cim;
                    uiTextBoxKeresztNev.Text = tanulo.Keresztnev;
                    uiTextBoxKor.Text = tanulo.Kor;
                    uiTextBoxVezetekNev.Text = tanulo.Vezeteknev;
                    x++;
                }
                else
                {
                    AddForm(tanulo.Vezeteknev, tanulo.Keresztnev, tanulo.Kor, tanulo.Cim);
                }
            }
        }

        public void AddForm(string vNev, string kNev, string kor, string cim)
        {
            WrapPanel uiMainWrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Width = 750,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            WrapPanel uiInnerWrapPanel1 = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                }
            };

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Vezetéknév: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxVezetekNev",
                    Width = 250,
                    Height = 25,
                    Text = vNev
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
                    Height = 25,
                    Text = kNev
                });

            WrapPanel uiInnerWrapPanel2 = new WrapPanel()
            {
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
                    Height = 25,
                    Text = kor
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
                    Height = 25,
                    Text = cim
                });
            BlockUIContainer uiContainer = new BlockUIContainer()
            {
                Child = uiMainWrapPanel,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
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
        
        public TanuloKontener deserialize(string xml)
        {
            TanuloKontener result = null;
            XmlSerializer serializer = new XmlSerializer(typeof(TanuloKontener));
            using (TextReader reader = new StringReader(xml))
            {
                result = (TanuloKontener)serializer.Deserialize(reader);
            }
            return result;
        }

        public void UploadTanuloKontener(TanuloKontener kontener)
        {
            SqlConnection sqlConnection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);

            sqlConnection.Open();

            SetByNamesResponse setByNamesResponse =
                new Ac4yObjectObjectService(sqlConnection).SetByNames(
                    new SetByNamesRequest() {
                        TemplateName = "tanuló konténer",
                        Name = "teszt"
                    });

            foreach(var tanulo in kontener.TanuloLista)
            {
                SetByNamesResponse setByNamesResponseTanulo =
                    new Ac4yObjectObjectService(sqlConnection).SetByNames(
                        new SetByNamesRequest()
                        {
                            TemplateName = "tanuló",
                            Name = tanulo.Vezeteknev + "." + tanulo.Keresztnev,
                        });
            }

            string xml = serialize(kontener, typeof(TanuloKontener));
            string GUID = setByNamesResponse.Ac4yObject.GUID;
        }

        public void UploadKontenerTanuloAssociation(TanuloKontener kontener)
        {
            SqlConnection sqlConnection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);

            foreach (var tanulo in kontener.TanuloLista) {
                Ac4yAssociationObjectService.SetByNamesResponse setByNamesResponse =
                    new Ac4yAssociationObjectService(sqlConnection).SetByNames(
                        new Ac4yAssociationObjectService.SetByNamesRequest()
                        {
                            AssociationPathName = "tanuló konténer.tanuló",
                            OriginTemplateName = "tanuló konténer",
                            OriginName = "teszt",
                            TargetTemplateName = "tanuló",
                            TargetName = tanulo.Vezeteknev + "." + tanulo.Keresztnev
                        });
            }
        }

        public void DeleteTanuloKontener(Tanulo tanulo)
        {
            SqlConnection connection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);
            connection.Open();

            
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
