using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSMUI = Tekla.Structures.Model.UI;
using TSDUI = Tekla.Structures.Drawing.UI;
using Tekla.Structures.Model;
using TS = Tekla.Structures;

namespace Tekla_Api_Register_Event
{
    public partial class Form1 : Form
    {
        private TSM.Events _eventsModel = new TSM.Events();
        private TSD.Events _eventsDrawing = new TSD.Events();
        private TSDUI.Events _eventsDrawingUI = new TSDUI.Events();


        private object _selectionEventHandlerLock = new object();
        private TSM.Model _model;
        private TSMUI.ModelObjectSelector _modelObjectSelector;

        int countClash = 0;
        public Form1()
        {
            InitializeComponent();
            _model = new TSM.Model();
            _modelObjectSelector = new TSMUI.ModelObjectSelector();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TSM.ClashCheckHandler clashCheckHandler = _model.GetClashCheckHandler();
            clashCheckHandler.RunClashCheck();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _eventsModel.ModelSave += Events_SaveModelEvent;
            _eventsModel.SelectionChange += Events_SelectionChangeEvent;
            _eventsModel.ClashCheckDone += Events_ClashCheckDone;
            _eventsModel.ClashDetected += Events_ClashDetected;
            _eventsModel.Register();

            _eventsDrawing.DrawingChanged += Events_DrawingChangedEvent;
            _eventsDrawing.DrawingDeleted += Events_DrawingDeletedEvent;
            _eventsDrawing.DrawingInserted += Events_DrawingInsertedEvent;
            _eventsDrawing.DrawingStatusChanged += Events_DrawingStatusChangedEvent;
            _eventsDrawing.DrawingUpdated += Events_DrawingUpdatedEvent;
            _eventsDrawing.Register();


            _eventsDrawingUI.DrawingLoaded += Events_DrawingLoad;
            _eventsDrawingUI.Register();
        }

        private void Events_ClashDetected(ClashCheckData ClashData)
        {
            countClash += 1;
            
            MessageBox.Show("Коллизия");
        }

        private void Events_ClashCheckDone(int NumbersOfClashes)
        {
            MessageBox.Show($"Проверка на коллизии завершена, найдено {countClash}");
        }

        private void Events_DrawingLoad()
        {
            lock (_selectionEventHandlerLock)
            {

                MessageBox.Show("Events_DrawingLoad");
            }
        }

        private void Events_DrawingUpdatedEvent(TSD.Drawing drawing, TSD.Events.DrawingUpdateTypeEnum type)
        {
            lock (_selectionEventHandlerLock)
            {

                MessageBox.Show("DrawingUpdatedEvent");
            }
        }

        private void Events_SelectionChangeEvent()
        {
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("SelectionChangeEvent");
                var selector = _modelObjectSelector.GetSelectedObjects();
                while(selector.MoveNext())
                {
                    if(selector.Current is TSM.Part part)
                    {
                        string profile = part.Profile.ProfileString;
                        
                    }

                }
            }
        }

        
        private void Events_DrawingStatusChangedEvent()
        {
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("DrawingStatusChangedEvent");
            }
        }

        private void Events_DrawingInsertedEvent()
        {
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("DrawingInsertedEvent");
            }
        }

        private void Events_DrawingDeletedEvent()
        {
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("DrawingDeletedEvent");
            }
        }

        private void Events_DrawingChangedEvent()
        {
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("DrawingChangedEvent");
            }
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _eventsModel.UnRegister();
            _eventsDrawing.UnRegister();
            _eventsDrawingUI.UnRegister();
        }

        private void Events_SaveModelEvent()
        {
            //Убеждаемся что код внутри блока работает синхронно с основным приложением.
            lock (_selectionEventHandlerLock)
            {
                MessageBox.Show("SaveModelEvent");
            }
        }
    }
}
