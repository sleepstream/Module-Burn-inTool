using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Media;
using Microsoft.Office.Interop.Excel;

namespace Module_Burn_inTool
{
   
    public partial class DiodTestForm : Form
    {
        mainForm form1 = (mainForm)System.Windows.Forms.Application.OpenForms["mainForm"];
        DateTime startWaitTime;
        int CurrentPD3;
        //int StartValuePD3;
        int newEterationPD3;
        int StartValuePD3;
        int LastPD3;
        bool startWait;
        bool stopWait=false;
        bool PD3DropError;
        int DiodCount;
        decimal oldAveraging;
        string logDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Stability\\PLDTestReports\\";
        string logDirectory = Module_Burn_inTool.Properties.Settings.Default.logDirectory + "PLDTestReports\\";
        int currentValue;
        int maxValue;
        int deviationDeltaValue = 15; //отклоненние deltaPD3  от среднего в процентах  по формуле (PD3DropNom- PD3DropReal)/ (PD3DropNom + PD3DropReal)/2
        int averageValuePD3Drop;
        bool Cancel;


        public DiodTestForm()
        {
            InitializeComponent();
            form1.checkDiodRun = true;
        }

        private void DiodTestForm_Load(object sender, EventArgs e)
        {
            form1.changeEvStatusLbl = 0;
            switch (form1.IGFLAG)
            {
                case 0:
                    TimeWaitPD3Check.Value = 2;
                    waitBeforNext.Value = 2;
                    break;
                case 1:
                    TimeWaitPD3Check.Value = 1;
                    waitBeforNext.Value = 1;
                    break;
            }
            form1.statusLblText("Запущена прозвонка дидов!", 2);
            if (System.IO.File.Exists("template.xlsx") == false)
            {
                MessageBox.Show("No template found. Creating new template file...");
                System.IO.StreamWriter OutStream;
                System.IO.BinaryWriter BinStream;

                OutStream = new System.IO.StreamWriter("template.xlsx", false);
                BinStream = new System.IO.BinaryWriter(OutStream.BaseStream);

                BinStream.Write(Module_Burn_inTool.Properties.Resources.template);
                BinStream.Close();
            }
            txtReportFile.Text = logDirectory;
            SetPD3Drop.Value = Module_Burn_inTool.Properties.Settings.Default.SetPD3Drop;
            TimeWaitPD3Check.Value = Module_Burn_inTool.Properties.Settings.Default.TimeWaitPD3Check;
            if (form1.averagingPD3_Value > 1)
            {
                SystemSounds.Beep.Play();
                oldAveraging = form1.averagingPD3_Value;
                form1.averagingPD3_Value = (decimal)1;
            }
            deviationDeltaPD3.Value = deviationDeltaValue;
            
        }

        public void startCheckDiod()
        {
            CurrentPD3=form1.PD3Text;
            if ((newEterationPD3 - CurrentPD3) > Convert.ToDouble(SetPD3Drop.Value) && CurrentPD3 > 10)// проверка на снижение PD3 ниже критического
            {
                textBox3.Text = "Значение PD3 снижается";
                if (CurrentPD3 < LastPD3 - DevitationPD3.Value && !stopWait)//  проверка продолжает ли снижаться значение PD3
                {
                    textBox3.Text = "Значение PD3 еще снижается";
                    LastPD3 = CurrentPD3;
                    startWait = false;
                }
                    //проверка что  PD3 устоялся в пределах 
                else if ((CurrentPD3 >= LastPD3 - DevitationPD3.Value && CurrentPD3 <= LastPD3 + DevitationPD3.Value) && !startWait && !stopWait)
                {
                    textBox3.Text = "PD3 стабилизировался, ждем";
                    startWaitTime = DateTime.Now;
                    startWait = true;
                }
                    //проверка что PD3 устоялся и запущено время выдержки на нижнем значении
                else if ((CurrentPD3 >= LastPD3 - DevitationPD3.Value && CurrentPD3 <= LastPD3 + DevitationPD3.Value) && startWait && !stopWait)
                {
                    textBox3.Text = "Подождали достаточно, записываем";
                    if (DateTime.Now.Subtract(startWaitTime).TotalSeconds >= Convert.ToDouble(TimeWaitPD3Check.Value))
                    {
                        //MessageBox.Show(CurrentPD3.ToString());
                        dataGridView1.Rows.Add(DiodCount, newEterationPD3, CurrentPD3, newEterationPD3 - CurrentPD3);
                        SystemSounds.Exclamation.Play();
                        DiodCount += 1;
                        stopWait = true;
                        startWaitTime = DateTime.Now;
                        btnReport.Enabled = true;
                    }
                }
                    //проверка что значение PD3 начало рости и просадка уже зафиксирована
                else if (CurrentPD3 > LastPD3 + DevitationPD3.Value && stopWait)
                {
                    textBox3.Text = "PD3 растет";
                    //если значение выросло не дойдя до исходного то нужно все перемерить
                    if(DateTime.Now.Subtract(startWaitTime).TotalSeconds >= Convert.ToDouble(TimeWaitPD3Check.Value)+5)
                    {
                        textBox3.Text = "PD3 не достиг стартового значения";
                        PD3DropError = true;
                    }
                }
                    //  в случае повторного замыкания того же диода сбрасываем время ожидания следующей итерации
                else if (CurrentPD3 < LastPD3 - DevitationPD3.Value && stopWait)
                {
                    startWaitTime = DateTime.Now;
                }
            }
            //выжидаем некоторое время прежде чем запустить новую итерацию (успей разомкнуть диод) проверка на отключение модуля
            else if (startWait && DateTime.Now.Subtract(startWaitTime).TotalSeconds >= Convert.ToDouble(waitBeforNext.Value) && CurrentPD3 > 500)
            {
                SystemSounds.Beep.Play();
                textBox3.Text = "new eteration PD3";
                newEterationPD3 = CurrentPD3;
                LastPD3 = CurrentPD3;
                startWait = false;
                stopWait = false;
            }
            //textBox3.Text = "";
        }

        private void DiodTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Module_Burn_inTool.Properties.Settings.Default.SetPD3Drop=SetPD3Drop.Value;
            Module_Burn_inTool.Properties.Settings.Default.TimeWaitPD3Check=TimeWaitPD3Check.Value;
            form1.checkDiodRun = false;
            form1.averagingPD3_Value = oldAveraging;
            form1.changeEvStatusLbl = 1;
            form1.statusLblText(" ", 2);
        }

        private void btnStartCheck_Click(object sender, EventArgs e)
        {
            
            if (btnStartCheck.Text == "Start Test")
            {
                if (dataGridView1.Rows.Count > 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Запустить новый тест?\nТекущий замер будет сброшен!", "Сброс текущих значений", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                            Cancel = true;
                        else
                        {
                            Cancel = false;
                        }
                }
                if (!Cancel)
                {
                    btnReport.Enabled = false;
                    dataGridView1.Rows.Clear();
                    DiodCount = 1;
                    StartValuePD3 = form1.PD3Text;
                    newEterationPD3 = form1.PD3Text;
                    LastPD3 = newEterationPD3;
                    PD3DropError = false;
                    startWait = false;
                    stopWait = false;
                    tmrCheck.Enabled = true;
                    btnStartCheck.Text = "Stop Test";
                }
                //фокус на таблицу для запуска обработчика по нажатию кнопки на клавиатуре
                dataGridView1.Focus();
            }
            else
            {

                tmrCheck.Enabled = false;
                checkValues();
                btnStartCheck.Text = "Start Test";
            }
        }

        private void checkValues()
        {
                    //среднее значение 
                    int count = 0;
                    double sumCurrentValue=0;
                    averageValuePD3Drop = 0;
                    double percent;
                    maxValue = 0;
            //ищем максимальное значение просадки диода
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        currentValue = Convert.ToInt32(row.Cells[1].Value);
                        if (currentValue > 0)
                        {
                            sumCurrentValue += currentValue;
                            count += 1;
                            /*if (currentValue > maxValue)
                                maxValue = currentValue;*/
                        }
                    }
                    //MessageBox.Show(sumCurrentValue + " - " + count);
                    if (count != 0)
                    {
                        //считаем величину просадки PD3 одного диода
                        averageValuePD3Drop = Convert.ToInt32(Math.Round((sumCurrentValue / count / count)));

                        // если среднее значение сильно занижено из-за просадки большого числа диодов.
                        /*if (averageValue - maxValue > deviationDeltaValue)
                            averageValue = maxValue;*/

                        //отмечаем отказавшие диоды
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            //отклонение от среднего в процентах
                            percent = 100 * (averageValuePD3Drop - Convert.ToInt32(row.Cells[3].Value)) / ((averageValuePD3Drop + Convert.ToInt32(row.Cells[3].Value))/2);
                            if (percent > deviationDeltaValue)
                                row.Cells[3].Style.BackColor = System.Drawing.Color.Red;
                            else
                                row.Cells[3].Style.BackColor = System.Drawing.Color.White;
                        }
                    }
        }
        private void tmrCheck_Tick(object sender, EventArgs e)
        {
            if (!PD3DropError)
            {
                startCheckDiod();
            }
            else
            {
                SystemSounds.Asterisk.Play();
                btnStartCheck_Click(this, new EventArgs());
                MessageBox.Show("PD3 Drop! Restart Test!");
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string WriteFile;
            bool fileEx = false;
            //try
            //{

                    if (!Directory.Exists(logDirectory))
                    {
                       // MessageBox.Show(logDirectory);
                        logDirectory = logDirectory1;

                        if (!Directory.Exists(logDirectory))
                        {
                            Directory.CreateDirectory(logDirectory);

                        }
                        fileEx = true;
                    }
                    else
                        fileEx = true;
                    if (fileEx)
                    {
                        txtReportFile.Text = logDirectory + form1.DataMod.ModuleID + " PLDTest.xlsx";
                        WriteFile = txtReportFile.Text;
                        Object FileName = WriteFile;
                        Object Missing = Type.Missing;
                        Microsoft.Office.Interop.Excel.Workbook xlWbSource;
                        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.Workbook wb;
                        Microsoft.Office.Interop.Excel.Range excelcells;
                        
                        if (!File.Exists(WriteFile))
                        {
                            File.Copy("template.xlsx", WriteFile, false);
                             wb = app.Workbooks.Open(WriteFile, Missing, Missing, Missing, 
                                Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                        }
                        else
                        {
                            File.Copy("template.xlsx", logDirectory + "template.xlsx", true);
                            wb = app.Workbooks.Open(WriteFile, Missing, Missing, Missing, 
                                Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                            //вставляем новый лист из шаблона в существующий файл протокола
                            xlWbSource = app.Workbooks.Open(logDirectory + "template.xlsx");
                            (xlWbSource.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet).Copy(Before: wb.Worksheets[1]);
                            xlWbSource.Close();
                            wb.Save();
                        }
                        
                        //file copied

                        //имя листа текущее время и дата
                        (wb.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet).Name = DateTime.Now.ToString("HH.mm.ss dd'.'MM'.'yyyy");
                        //date
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[DATE]", DateTime.Now.ToShortDateString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        //SN
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[SNUMBER]", form1.DataMod.ModuleID, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        //emission time
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[EMTIME1]", form1.txtCycleTime.Text, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[EMTIME2]", form1.txtFullTime.Text, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        //operator
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[OPERATOR]", Environment.UserName, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                        //DELTA
                        ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[DELTA]", averageValuePD3Drop, XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);

                        //fill digital
                        for (int i = 1; i <= 36; i++)
                        {
                            if (i <= dataGridView1.Rows.Count)
                            {
                                if (dataGridView1[3, i - 1].Style.BackColor == Color.Red)
                                {
                                    excelcells=((Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet).Cells.Find("[D" + (i).ToString("F00") + "Delta]");
                                    excelcells.Interior.Color = Color.Red;
                                }
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Start]", dataGridView1[1, i - 1].Value.ToString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Stop]", dataGridView1[2, i - 1].Value.ToString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Delta]", dataGridView1[3, i - 1].Value.ToString(), XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                            }
                            else
                            {
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Start]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Stop]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                                ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1]).Cells.Replace("[D" + (i).ToString("F00") + "Delta]", "", XlLookAt.xlWhole, XlSearchOrder.xlByRows, false, false, true, false);
                            }

                        }

                        app.ActiveWorkbook.Save();
                        app.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Невозможно записать отчет");
                    }
            /* }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             */
        }

        private void btnReportFile_Click(object sender, EventArgs e)
        {
            sfdSaveReport.AddExtension = true;
            sfdSaveReport.DefaultExt = ".xlsx";
            sfdSaveReport.FileName = form1.DataMod.ModuleID + " PLDTest";
            //sfdSaveReport.DefaultExt = "xlsx";
            sfdSaveReport.Filter = "xlsx files (*.xlsx)|*.xlsx";
            if (sfdSaveReport.ShowDialog() == DialogResult.OK)
                txtReportFile.Text = sfdSaveReport.FileName;
            logDirectory = txtReportFile.Text;
        }

        private void DiodTestForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar.ToString() == " " && btnStartCheck.Text == "Stop Test")
            {
                dataGridView1.Rows.Add(DiodCount, newEterationPD3, CurrentPD3, newEterationPD3 - CurrentPD3);
                SystemSounds.Beep.Play();
                textBox3.Text = "new eteration PD3";
                newEterationPD3 = CurrentPD3;
                LastPD3 = CurrentPD3;
                startWait = false;
                stopWait = false;
                DiodCount += 1;
                btnReport.Enabled = true;
            }
        }

        private void deviationDeltaPD3_ValueChanged(object sender, EventArgs e)
        {
            deviationDeltaValue = Convert.ToInt32(deviationDeltaPD3.Value);
            checkValues();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory + "\\template.xlsx");
        }
    }
    class SampleRow
    {
        public int Number { get; set; } //обязательно нужно использовать get конструкцию
        public int StartPD3 { get; set; }
        public int DropPD3 { get; set; }

        //public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public SampleRow(int Number, int StartPD3, int dropPD3)
        {
            this.Number = Number;
            this.StartPD3 = StartPD3;
            this.DropPD3 = dropPD3;
        }
    }
}
