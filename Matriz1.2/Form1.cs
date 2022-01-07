using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Matriz1._2
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        #region v:
        #region variables
        public int orden = 1;
        public int fil = 0, col = 0, ma = 0, na = 0, mb = 0, nb = 0, ra = 0, rb = 0, r = 0, vec = 0;
        public double[,] matInv = new double[100, 200];
        public double[,] oper_bas_a = new double[100, 100];
        public double[,] oper_bas_b = new double[100, 100];
        public double[,] oper_bas_res = new double[100, 100];
        /*
         * para visualizar en un nuevo formulario, (ventana), se debe crear un objeto que contenga este formulario que en este caso llamamos paso
         */
        frm_pap paso = new frm_pap();
        /*
         * para generar los números randómicos, se crea un objeto de la clase Random de nombre rnd
         */
        Random rnd = new Random();
        string cadena = null;
        #endregion
        /*
         * método que se ejecuta una sola vez cuando empieza a correr el programa
         */
        public Form1()
        {
            InitializeComponent();
            size();
            generar_tablas();
            generarsistema();
        }
        /*
         * método que permite a cada elemento dentro del formulario, cambiar su tamaño con proporcion al formulario
         */
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            size();
        }
        /*
         * evento que genera el random de la matriz inversa
         */
        private void btn_randomInversa_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            int i = 0, j = 0;
            orden = Convert.ToInt32(txt_ordenInversa.Text);
            dgv_inversa_a.ColumnCount = orden;
            dgv_inversa_a.RowCount = orden;
            dgv_inversa_a.RowHeadersVisible = false;
            dgv_inversa_a.ColumnHeadersVisible = false;
            dgv_inversa_b.ColumnCount = orden;
            dgv_inversa_b.RowCount = orden;
            dgv_inversa_b.RowHeadersVisible = false;
            dgv_inversa_b.ColumnHeadersVisible = false;
            #region random
            for (i = 0; i < orden; i++)
            {
                for (j = 0; j < orden; j++)
                {
                    dgv_inversa_a[j, i].Value = rnd.Next(-100, 101);
                }
            }
            #endregion
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }
        /*
         * evento que genera el calculo de la inversa
         */
        private void btn_cal_inv_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            /*
             * Variables Para calculo de inversa y determinante
             */
            double[] aux = null;
            double[] con = null;
            double[] val_det = null;
            aux = new double[orden * 2];
            con = new double[orden];
            val_det = new double[orden];
            double au = 0, au1 = 0, det = -1; ;
            bool cond = true, ordenar = true;
            int i = 0, j = 0, k = 0, l = 0, o = 0,z=0;
            k = 0;
            det = 1;
            txt_det.Text = "";
            /*
             * reinicio del arreglo con todos los valores de cero
             */
            #region reiniciar matriz
            for (i = 0; i < orden; i++)
            {
                for (j = 0; j < orden * 2; j++)
                {
                    matInv[i, j] = 0;
                }
            }
            #endregion
            /*
             * llenado de la matriz segun los valores del datagridView
             */
            #region llenado de matriz desde gridview
            for (i = 0; i < orden; i++)
            {
                for (j = 0; j < orden; j++)
                {
                    try
                    {
                        matInv[i, j] = Convert.ToDouble(dgv_inversa_a[j, i].Value);
                    }
                    catch(Exception)
                    {
                        cond = false;
                    }
                }
                /*
                 * matriz identidad
                 */
                for (j = 0; j < orden * 2; j++)
                {
                    if (j == (i + orden))
                    {
                        matInv[i, j] = 1;
                    }
                }
            }
            #endregion
            #region llenado de la ventana paso a paso y calculo de la inversa
            /*
             * llenado de la ventana del paso a paso 
             */
            cadena = "La Matriz de orden " + Convert.ToString(orden) + " \n";
            for (i = 0; i < orden; i++)
            {
                for (j = 0; j < orden; j++)
                {
                    cadena = cadena + Convert.ToString(dgv_inversa_a[j, i].Value) + "\t";
                }
                cadena = cadena + "\n";
            }
            cadena = cadena + "\n";
            #region Restricciones Inversa
            //if()
            #region restriccion columnas
            for (i = 0; i < orden; i++)
            {
                k = 0;
                for (j = 0; j < orden; j++)
                {
                    if (matInv[j, i] == 0)
                        k = k + 1;
                    if (k == orden)
                        cond = false;
                }
                k = 0;
                for (j = 0; j < orden; j++)
                {
                    if (matInv[i, j] == 0)
                        k = k + 1;
                    if (k == orden)
                        cond = false;
                }
                k = 0;
                for (j = 0; j < orden; j++)
                {
                    if (matInv[j, i] == 0)
                        k = k + 1;
                    if (k == orden)
                        cond = false;
                }
            }
            #endregion
            #region restriccion de equivalencias
            for (i = 0; i < orden - 1; i++)
            {
                k = 0;
                for (j = i + 1; j < orden; j++)
                {
                    k = 0;
                    for (l = 0; l < orden; l++)
                    {
                        con[l] = matInv[i, l] / matInv[j, l];
                    }
                    k = 0;
                    for (l = 1; l < orden; l++)
                    {
                        if (con[0] == con[l])
                        {
                            k++;
                        }
                        if (k == orden - 1)
                            cond = false;
                    }
                }
            }
            #endregion
            if (cond == false)
            {
                cadena = cadena + "No tiene inversa alguna :/ \n\nIntentalo con otros valores\nProcura que ninguna de las columnas este con todos sus elementos cero o que no sean equivalentes entre filas\nO que sean números lo que ingreses v:";
            }
            if (cond == true)
            {
                cadena = cadena + "Tiene una matriz inversa\n\nAdyacente a la matriz anterior, se genera la matriz identidad del mismo orden:  " + Convert.ToString(orden) + "\n";
                visualizar_Inversa(matInv, orden, orden * 2);
            }
            #endregion
            #region calculo inversa
            if (cond == true)
            {
                /*
                 * ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                 */
                while (ordenar == true)
                {
                    for (i = 0; i < orden; i++)
                    {
                        if (matInv[i, i] == 0)
                        {
                            ordenar = true;
                            cadena = cadena + "\nSe reordena de manera conveniente\n";
                            for (j = 0; j < orden; j++)
                            {
                                if (j != i && matInv[j, i] != 0)
                                {
                                    o++;
                                    for (k = 0; k < orden * 2; k++)
                                    {
                                        aux[k] = matInv[j, k];
                                        matInv[j, k] = matInv[i, k];
                                        matInv[i, k] = aux[k];
                                    }
                                }
                            }
                            visualizar_Inversa(matInv, orden, orden * 2);
                            det = Math.Pow(-1, o);
                        }
                        else
                            ordenar = false;
                    }
                }
                /*--------------------------------------------------------------------------------------------------*/
                for (i = 0; i < orden; i++)
                {
                    au = matInv[i, i];
                    val_det[i] = au;
                    det = det * au;
                    for (j = 0; j < orden * 2; j++)
                    {
                        matInv[i, j] = matInv[i, j] / au;
                    }
                    cadena = cadena + "\nSe divide la " + Convert.ToString(i + 1) + "° fila para " + Convert.ToString(au) + "\n";
                    visualizar_Inversa(matInv, orden, orden * 2);
                    cadena = cadena + "\n";
                    for (j = 0; j < orden; j++)
                    {
                        if (i != j)
                        {
                            au1 = matInv[j, i];
                            for (k = 0; k < orden * 2; k++)
                            {
                                matInv[j, k] = matInv[j, k] - au1 * matInv[i, k];
                            }
                        }
                    }
                    if (i <= orden - 1)
                    {
                        cadena = cadena + "\nSe suma algebraicamente los elementos de la fila " + Convert.ToString(i + 1) + " con los elementos de las filas restantes, para dejar en la columna unicamente la unidad, parte de la matriz identidad\n";
                        visualizar_Inversa(matInv, orden, orden * 2);
                    }
                    cadena = cadena + "\n";
                    txt_det.Text = Convert.ToString(det);
                }
                cadena = cadena + "\nDETERMINANTE\nPara evaluar la determinante, se toma los valores de la diagonal principal y se multiplica dichos valores\n";
                cadena = cadena + "\nSi se necesita intercambiar filas en la matriz para evaluar el determinante, se aplica: det A= (-1)^n * d1 * d2 * d3... * dm, donde: n es el número de ordenamientos, y m: es el tamaño de la matriz\n";
                cadena = cadena + "\nSe hicieron " + Convert.ToString(o) + " ordenamientos, y (-1)^" + Convert.ToString(o) + " es: " + Convert.ToString(Math.Pow(-1, o)) + "\n";
                cadena = cadena + Convert.ToString(Math.Pow(-1, o)) + " * ";
                for (i = 0; i < orden - 1; i++)
                {
                    cadena = cadena + Convert.ToString(val_det[i]) + " * ";
                }
                cadena = cadena + Convert.ToString(val_det[orden - 1]) + " = " + Convert.ToString(det);
                cadena = cadena + "\n";
            }
            #endregion
            #endregion
            #region llenado datagridviwe respuesta
            /*
             * llenado del datagridview de la solucion
             */
            for (i = 0; i < orden; i++)
            {
                for (j = orden; j < orden * 2; j++)
                {
                    dgv_inversa_b[j - orden, i].Value = matInv[i, j].ToString("f4");
                }
            }
            #endregion
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }
        /*
         * evento que permite visualizar en la ventana Paso a Paso.
         */
        private void btn_pap_inv_Click(object sender, EventArgs e)
        {
            paso.Show();
            ((RichTextBox)paso.Controls["richTextBox_pap"]).Text = cadena;
            paso.Focus();
        }
        /*
         * evento para visualizar la inversa en la ventana de paso a paso
         */
        private void visualizar_Inversa(double[,] mat, int fil, int col)
        {
            int x = 0, y = 0;
            for (y = 0; y < fil; y++)
            {
                for (x = 0; x < col; x++)
                {
                    if (x == orden)
                    {
                        cadena = cadena + "|\t";
                    }
                    cadena = cadena + (mat[y, x]).ToString("f4") + "\t";
                }
                cadena = cadena + "\n";
            }
        }
        /*
         * evento que conlleva a las relaciones de posicion, y tamaño de los elementos del form
         */
        private void size()
        {
            /*
             * se crea objeto de tipo Size que abarcará el tamaño de los datagridView, 
             * y estos dependerán del tamaño del groupBox, que este a su vez dependerá
             * del tamaño de la ventana
             * se crea objeto de tipo Point para poder tener un origen donde se ubicará
             * el segundo datagridView
             */
            /*
             *--------------------------Auto Resizable----------------------------*
             */
            Size t_m_i_a = new Size((groupBox1.Size.Width - 15) / 2, groupBox1.Size.Height - 30);
            Point l_m_i_a = new Point((groupBox1.Size.Width - 15) / 2 + 10, 19);
            dgv_inversa_b.Location = l_m_i_a;
            dgv_inversa_b.Size = t_m_i_a;
            dgv_inversa_a.Size = t_m_i_a;

            Size t_m_d_a = new Size((groupBox3.Size.Width - 15) / 2, groupBox3.Size.Height - 30);
            Point l_m_d_a = new Point((groupBox3.Size.Width - 15) / 2 + 10 + 5, 19);
            dgv_sis_b.Location = l_m_d_a;
            dgv_sis_a.Size = t_m_d_a;
            dgv_sis_b.Size = t_m_d_a;

            Size t_m_o_a = new Size((groupBox4.Size.Width - 15) / 2, (groupBox4.Size.Height - 15) / 2);
            Point l_m_o_b = new Point(5, (groupBox4.Size.Height - 15) / 2 + 10);
            groupBox6.Location = l_m_o_b;
            groupBox5.Size = t_m_o_a;
            groupBox6.Size = t_m_o_a;/**/
            Size t_m_o_r = new Size((groupBox4.Size.Width - 15) / 2, (groupBox4.Size.Height - groupBox9.Size.Height- 15 - 5));
            Point l_m_o_r = new Point((groupBox4.Size.Width - 15) / 2 + 10, groupBox9.Size.Height + 15);
            groupBox8.Size = t_m_o_r;
            groupBox8.Location = l_m_o_r;
            groupBox9.Location = new Point((groupBox4.Size.Width - 15) / 2 + 10, 10);

        }
        /*
        * evento que permite modificar el tamaño de los ementos a cada cambio de pestaña
        */

        private void tab_mat_SelectedIndexChanged(object sender, EventArgs e)
        {
            size();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }
        private void ShowAbout()
        {
            AboutBoxDemo.AboutBox ab = new AboutBoxDemo.AboutBox();
            ab.AppTitle = "Algebra Lineal";
            ab.AppDescription = "Programa Educativo";
            ab.AppVersion = "Versión 1.2";
            ab.AppCopyright = "Copyright © %year%, Pacman Feliz v:";
            //aqui la cadena de informacion de la aplicacion
            ab.AppMoreInfo = "%product% is %copyright%, Pacman Feliz v: Desarrollador de Pacman by Solomon Aeternus ;)\nTrabajo conjunto, de resolucion de matrices, calculos de inversa, determinantes, operaciones básicas, sistemas de ecuaciones, etc\nEn asociacion a la facultad de Ingenieria, Ciencias Fisicas y Matematicas de la Universidad Central del Ecuador (UCE) Con sus Respectivos Integrantes, quienes encomendaron la laboriosa tarea de diseñar interfaz y algoritmos, e interpretacion de datos a Solomon, desarrollador de la facultad de Ciencias de la Ingenieria e Industrias, Ingeniería Mecatrónica, de la Universidad Tecnológica Equinoccial (UTE).\nPara información y/o dudas sobre cómo se desarrolló el código, y el programa, comuniquese a uno de los siguientes correos:\ncar1994los@hotmail.com\ncarlosanatanazz@gmail.com\nbicj96474@ute.edu.ec\nO al teléfono celular, o Whatsapp : \n+593987264194\nQuito - Ecuador\nHecho por Solomon ;)\nNo hay que olvidar los agradecimientos a los Desarrolladores de complementos como MetroFramework© por el diseño moderno de la UI y a Coding Horror por permitirme usar esta ventana :v\nSe corrigió los pequeños bug que hacian que la resolucion paso a paso sea muy tardia, optimizandola en mas de la mitad del tiempo de proceso.\nSe agregó la posibilidad de pegar celdas desde el mismo programa o excel\nSe arregló los pequeños Bugs de el algoritmo de ordenamiento de matrices\nPequeños ajustes en UI, etc, etc.";
            //prueba cambiando de true a false, y te quedas con elque te guste
            ab.AppDetailsButton = false;
            ab.ShowDialog(this);
        }

        private void txt_ordenInversa_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_col_sis_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_fil_sis_ValueChanged(object sender, EventArgs e)
        {
            
        }

        #region Eventos de llenar random sistema ecuaciones

        private void btn_ran_sis_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0, j = 0;
            generarsistema();
            for (i = 0; i < fil; i++)
            {
                for (j = 0; j < col+1; j++)
                {
                    dgv_sis_a[j, i].Value = rnd.Next(-100, 101);
                }
            }
            Cursor.Current = Cursors.Default;
        }
        #endregion
        /*
         * evento para generar las tablas (excepto sistema de ecuaciones)
         */
        private void generar_tablas()
        {
            /*
             * inversa
             */
            orden = Convert.ToInt32(txt_ordenInversa.Text);
            dgv_inversa_a.ColumnCount = orden;
            dgv_inversa_a.RowCount = orden;
            dgv_inversa_a.RowHeadersVisible = false;
            dgv_inversa_a.ColumnHeadersVisible = false;
            dgv_inversa_b.ColumnCount = orden;
            dgv_inversa_b.RowCount = orden;
            dgv_inversa_b.RowHeadersVisible = false;
            dgv_inversa_b.ColumnHeadersVisible = false;
            /*
             * operaciones
             */        
            ma = Convert.ToInt32(txt_col_a.Value);
            na = Convert.ToInt32(txt_fil_a.Value);
            dgv_m_ob_a.RowHeadersVisible = false;
            dgv_m_ob_a.ColumnHeadersVisible = false;
            dgv_m_ob_a.RowCount = na;
            dgv_m_ob_a.ColumnCount = ma;

            mb = Convert.ToInt32(txt_col_b.Value);
            nb = Convert.ToInt32(txt_fil_b.Value);
            dgv_m_ob_b.RowHeadersVisible = false;
            dgv_m_ob_b.ColumnHeadersVisible = false;
            dgv_m_ob_b.RowCount = nb;
            dgv_m_ob_b.ColumnCount = mb;

            if(ma==mb && na==nb)
            {
                btn_op_sum.Enabled = true;
                btn_op_res.Enabled = true;
            }
            else
            {
                btn_op_sum.Enabled = false;
                btn_op_res.Enabled = false;
            }

            if (ma == nb)
            {
                btn_op_mul_axb.Enabled = true;
            }
            else
            {
                btn_op_mul_axb.Enabled = false;
            }

            if (na == mb)
            {
                btn_op_mul_bxa.Enabled = true;
            }
            else
            {
                btn_op_mul_bxa.Enabled = false;
            }
            if(ma==na && rdb_ma.Checked==true)
            {
                btn_op_adjunta.Enabled = true;
            }
            else if (mb == nb && rdb_mb.Checked == true)
            {
                btn_op_adjunta.Enabled = true;
            }
            else 
            {
                btn_op_adjunta.Enabled = false;
            }
        }
        /*
         * evento para generar las tablas de los sistemas de ecuaciones
         */
        private void generarsistema()
        {
            int i = 0, j = 0;
            col = Convert.ToInt32(txt_col_sis.Value);
            fil = Convert.ToInt32(txt_fil_sis.Value);
            dgv_sis_a.ColumnCount = col + 1;
            dgv_sis_a.RowCount = fil;
            dgv_sis_a.RowHeadersVisible = false;
            dgv_sis_a.ColumnHeadersVisible = true;
            dgv_sis_b.ColumnCount = col + 1;
            dgv_sis_b.RowCount = fil;
            dgv_sis_b.RowHeadersVisible = false;
            dgv_sis_b.ColumnHeadersVisible = true;
            for (i = 0; i < col; i++)
            {
                dgv_sis_a.Columns[i].HeaderText = "X" + Convert.ToString(i + 1);
                dgv_sis_b.Columns[i].HeaderText = " ";
                for (j = 0; j < fil; j++)
                {
                    if(j==i)
                        dgv_sis_b[j, i].Value = "X" + Convert.ToString(i + 1);
                }
            }
            dgv_sis_a.Columns[col].HeaderText = "=";
            dgv_sis_b.Columns[col].HeaderText = "=";
            if (fil == col)
                rdbCrammer.Enabled = true;
            else
                rdbCrammer.Enabled = false;
        }
        //evento click para calcular sistema de ecuaciones
        private void btn_calc_sis_Click(object sender, EventArgs e)
        {
            int i = 0, j = 0;
            double[,] matSis = new double[100, 101];
            double[,] solver = new double[100, 101];
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            for (i = 0; i < fil; i++)
            {
                for (j = 0; j < col + 1; j++)
                {
                    matSis[i, j] = 0;
                    solver[i, j] = 0;
                }
            }
            bool cond = false;
            cadena = "El sistema de ecuaciones de " + Convert.ToString(fil) + " ecuaciones, y de " + Convert.ToString(col) + " incognitas \n";
            for (i = 0; i < fil; i++)
            {
                for (j = 0; j <= col; j++)
                {
                    matSis[i, j] = Convert.ToDouble(dgv_sis_a[j, i].Value);
                }
            }
            for (i = 0; i < col; i++)
            {
                cadena = cadena + "X" + Convert.ToString(i + 1) + "\t";
            }
            cadena = cadena + "|\t=\n";
            for (i = 0; i < fil; i++)
            {
                for (j = 0; j < col + 1; j++)
                {
                    if (j == col)
                    {
                        cadena = cadena + "|\t";
                    }
                    cadena = cadena + Convert.ToString(matSis[i, j]) + "\t";
                }
                cadena = cadena + "\n";
            }
            if (fil > col)
            {
                cadena = cadena + "No presenta solución, es un sistema incosistente.\n";
                cond = false;
            }
            if (fil < col)
            {
                /*
                 * inconsistente, no solucion (se llega a una contradiccion)
                 */
                cadena = cadena + "Presenta solución infinita, es un sistema consistente con variables libres.\n";
                cond = false;
            }
            if (col == fil)
            {
                /*
                 * posible única solucion
                 */
                cond = true;
            }
            if (rdb_gauss.Checked == true && cond == true)
            {
                solver = sis_sol(matSis, fil, col + 1, 0);
                for (i = 0; i < fil; i++)
                {
                    for (j = 0; j < col + 1; j++)
                    {
                        if (i != j)
                        {
                            dgv_sis_b[j, i].Value = solver[i, j].ToString("f4");
                        }
                    }
                }
            }
            if (rdb_gaussJordan.Checked == true && cond == true)
            {
                solver = sis_sol(matSis, fil, col + 1, 1);
                for (i = 0; i < fil; i++)
                {
                    for (j = 0; j < col + 1; j++)
                    {
                        if (j > i)
                        {
                            dgv_sis_b[j, i].Value = solver[i, j].ToString("f4");
                        }
                    }
                }
            }
            if (rdbCrammer.Checked == true && cond == true)
            {
                solver = sis_sol(matSis, fil, col + 1, 2);
                for (i = 0; i < fil; i++)
                {
                    for (j = 0; j < col + 1; j++)
                    {
                        if (j > i)
                        {
                            dgv_sis_b[j, i].Value = solver[i, j].ToString("f4");
                        }
                    }
                }
            }
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }
        /*
         * evento para visualizar cualquier matriz con un separador como condicional
         */
        private void visualizar_matriz(double[,] mat0, int fila, int columna, bool separador)
        {
            int x = 0, y = 0;
            for (y = 0; y < fila; y++)
            {
                for (x = 0; x < columna; x++)
                {
                    if (x == columna - 1 && separador == true)
                    {
                        cadena = cadena + "|\t";
                    }
                    cadena = cadena + (mat0[y, x]).ToString("f4") + "\t";
                }
                cadena = cadena + "\n";
            }
        }
        /*
         * evento que calcula el sistema de ecuaciones dependiendo el método de resolucion (2/3)
         */
        private double[,] sis_sol(double[,] mata, int fila, int columna, int metodo)
        {
            int i = 0, j = 0, k = 0, l = 0, m = 0;
            double[] aux = new double[columna];
            //double[,] mat_original = new double[100, 101];
            double[,] mat1 = new double[100, 101];
            double[,] mat2 = new double[100, 101];
            double[,] crammer = new double[100, 101];
            for (i = 0; i < fila; i++)
            {
                for (j = 0; j < columna; j++)
                {
                    mat1[i, j] = mata[i, j];
                    crammer[i, j] = mata[i, j];
                    mat2[i, j] = mat1[i, j];
                }
            }
            if (metodo == 0 || metodo == 1)
            {
                double au = 0, au1 = 0, au2 = 0;
                bool ordenar = false;
                k = 0;
                for (i = 0; i < fila; i++)
                {
                    if (mat1[i, i] == 0)
                    {
                        ordenar = true;
                    }
                }
                if (ordenar == true)
                {
                    cadena = cadena + "\nSe reordena de manera conveniente\n";
                    for (i = 0; i < fila; i++)
                    {

                        if (mat1[i, i] == 0 && i < fila - 1)
                        {
                            for (j = 0; j < columna; j++)
                            {
                                aux[j] = mat1[i, j];
                                mat1[i, j] = mat1[i + 1, j];
                                mat1[i + 1, j] = aux[j];
                                mat2[i, j] = mat1[i, j];
                            }
                        }
                        if (mat1[i, i] == 0 && i == fila - 1)
                        {
                            for (j = 0; j < columna; j++)
                            {
                                aux[j] = mat1[i, j];
                                mat1[i, j] = mat1[i - 1, j];
                                mat1[i - 1, j] = aux[j];
                                mat2[i, j] = mat1[i, j];
                            }
                        }
                    }
                    visualizar_matriz(mat1, fila, columna, true);
                }
                /*--------------------------------------------------------------------------------------------------*/
                for (i = 0; i < fila; i++)
                {
                    au = mat1[i, i];
                    for (j = 0; j < columna; j++)
                    {
                        mat1[i, j] = mat1[i, j] / au;
                        mat2[i, j] = mat1[i, j];
                    }
                    cadena = cadena + "\nSe divide la " + Convert.ToString(i + 1) + "° fila para " + Convert.ToString(au) + "\n";
                    visualizar_matriz(mat1, fila, columna, true);
                    cadena = cadena + "\n";
                    if (metodo == 1)
                    {
                        for (j = 0; j < fila; j++)
                        {
                            if (i != j)
                            {
                                au1 = mat1[j, i];
                                for (k = 0; k < columna; k++)
                                {
                                    mat1[j, k] = mat1[j, k] - au1 * mat1[i, k];
                                }
                            }
                        }
                    }
                    if (metodo == 0)
                    {
                        for (l = 0; l < fila; l++)
                        {
                            if (i != l)
                            {
                                au2 = mat2[l, i];
                                for (m = 0; m < columna; m++)
                                {
                                    mat2[l, m] = mat2[l, m] - au2 * mat2[i, m];
                                }
                            }
                        }

                        for (j = i + 1; j < fila; j++)
                        {
                            au1 = mat1[j, i];
                            for (k = 0; k < columna; k++)
                            {
                                mat1[j, k] = mat1[j, k] - au1 * mat1[i, k];
                            }
                        }
                    }
                    if (i <= fila - 1)
                    {
                        cadena = cadena + "\nSe suma algebraicamente los elementos de la fila " + Convert.ToString(i + 1) + " con los elementos de las filas restantes, para dejar en la columna unicamente la unidad, parte de la matriz identidad\n";
                        visualizar_matriz(mat1, fila, columna, true);
                    }
                    cadena = cadena + "\n";
                }
                if (metodo == 0)
                {
                    //gauss
                    cadena = cadena + "\nA diferencia del Método de Gauss Jordan, el método de Gauss, nos da ya una incognita ya resuelta, lista para ser remplazada en la ecuacion anterior a esta, permitiendo despejar otra ecuacion más\n";
                    cadena = cadena + "\nEs decir la Incognita X" + Convert.ToString(fila) + " permite despejar la incognita previa a esta, X" + Convert.ToString(fila - 1) + " y esta a la anterior X" + Convert.ToString(fila - 2) + "\n";
                    cadena = cadena + "\nAsí sucesivamente hasta encontrar todas las incognitas que presenten solucion alguna\n";
                    cadena = cadena + "\nLa solucion para las incognitas son:\n";
                    for (i = 0; i < fila; i++)
                    {
                        cadena = cadena + "X" + Convert.ToString(i + 1) + " = " + mat2[i, fila].ToString("f4") + "\n";
                    }
                    cadena = cadena + "\n Para verificar dicho sistema, se reemplazará los valores encontrados en la primera ecuacion\n";
                    for (i = 0; i < columna - 2; i++)
                    {
                        cadena = cadena + mata[0, i].ToString("f4") + " X" + Convert.ToString(i + 1) + " + ";
                    }
                    cadena = cadena + mata[0, columna - 2].ToString("f4") + " X" + Convert.ToString(columna - 1) + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                    for (i = 0; i < columna - 2; i++)
                    {
                        cadena = cadena + mata[0, i].ToString("f4") + "(" + mat2[i, fila].ToString("f4") + ")" + " + ";
                    }
                    cadena = cadena + mata[0, columna - 2].ToString("f4") + "(" + mat2[columna - 2, fila].ToString("f4") + ")" + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                    double sum = 0;
                    for (i = 0; i < fila; i++)
                    {
                        sum = sum + mata[0, i] * mat2[i, fila];
                    }
                    cadena = cadena + sum.ToString("f4") + " ­≡ " + mata[0, columna - 1].ToString("f4") + "\n\n";
                }
                if (metodo == 1)
                {
                    //Gauss Jordan, mensaje bonico
                    cadena = cadena + "\nLa solucion para las incognitas son:\n";
                    for (i = 0; i < fila; i++)
                    {
                        cadena = cadena + "X" + Convert.ToString(i + 1) + " = " + mat1[i, fila].ToString("f4") + "\n";
                    }
                    cadena = cadena + "\n Para verificar dicho sistema, se reemplazará los valores encontrados en la primera ecuacion\n";
                    for (i = 0; i < columna - 2; i++)
                    {
                        cadena = cadena + mata[0, i].ToString("f4") + " X" + Convert.ToString(i + 1) + " + ";
                    }
                    cadena = cadena + mata[0, columna - 2].ToString("f4") + " X" + Convert.ToString(columna - 1) + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                    for (i = 0; i < columna - 2; i++)
                    {
                        cadena = cadena + mata[0, i].ToString("f4") + "(" + mat1[i, fila].ToString("f4") + ")" + " + ";
                    }
                    cadena = cadena + mata[0, columna - 2].ToString("f4") + "(" + mat1[columna - 2, fila].ToString("f4") + ")" + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                    double sum = 0;
                    for (i = 0; i < fila; i++)
                    {
                        sum = sum + mata[0, i] * mat1[i, fila];
                    }
                    cadena = cadena + Convert.ToString(sum) + " ≡ " + mata[0, columna - 1].ToString("f4") + "\n\n";
                }
            }
            if (metodo == 2)
            {
                //crammer, mensaje bonico
                double[,] determi = new double[fila, fila];
                double[] col_excluida = new double[fila];
                double[] col_igualdad = new double[fila];
                double det_principal = 0;
                double[] det_incognitas = new double[fila];
                cadena = cadena + "\nSe calcula la determinante de la matriz principal o llamada tambien matriz de coeficientes del sistema:\n";
                cadena = cadena + "\n";
                visualizar_matriz(crammer, fila, fila, false);
                det_principal = determinante(crammer, fila, 0);
                cadena = cadena + "\nDeterminante principal: " + det_principal.ToString("f4") + "\n";
                for (i = 0; i < fila; i++)
                {
                    for (j = 0; j < columna; j++)
                    {
                        mat1[i, j] = 0;
                    }
                }
                for (i = 0; i < fila; i++)
                {
                    for (j = 0; j < fila; j++)
                    {
                        col_excluida[j] = crammer[j, i];
                        col_igualdad[j] = crammer[j, col];
                    }
                    cadena = cadena + "\nSe reordena la matriz, intercambiando la columna de incognita X" + Convert.ToString(i + 1) + " con la columna de la igualdad:\n";
                    for (j = 0; j < fila; j++)
                    {
                        for (k = 0; k < fila; k++)
                        {
                            determi[j, k] = crammer[j, k];
                            if (k == i)
                            {
                                determi[j, k] = col_igualdad[j];
                            }
                        }
                    }
                    cadena = cadena + "\n";
                    visualizar_matriz(determi, fila, fila, false);
                    cadena = cadena + "\nSe calcula la determinante de dicha matriz: \n";
                    det_incognitas[i] = determinante(determi, fila, 0);
                    cadena = cadena + "det = " + det_incognitas[i].ToString("f4") + "\n";
                }
                cadena = cadena + "la solucion de las incógnitas, se calculan dividiendo cada determinante hallada por cada incógnita para el determinante principal \n";
                for (i = 0; i < fila; i++)
                {
                    mat1[i, fila] = det_incognitas[i] / det_principal;
                    cadena = cadena + "X" + Convert.ToString(i + 1) + " = " + det_incognitas[i].ToString("f4") + "/" + det_principal.ToString("f4") + " = " + mat1[i, fila].ToString("f4") + "\n";
                }
                /**/
                cadena = cadena + "\n Para verificar dicho sistema, se reemplazará los valores encontrados en la primera ecuacion\n";
                for (i = 0; i < columna - 2; i++)
                {
                    cadena = cadena + mata[0, i].ToString("f4") + " X" + Convert.ToString(i + 1) + " + ";
                }
                cadena = cadena + mata[0, columna - 2].ToString("f4") + " X" + Convert.ToString(columna - 1) + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                for (i = 0; i < columna - 2; i++)
                {
                    cadena = cadena + mata[0, i].ToString("f4") + "(" + mat1[i, fila].ToString("f4") + ")" + " + ";
                }
                cadena = cadena + mata[0, columna - 2].ToString("f4") + "(" + mat1[columna - 2, fila].ToString("f4") + ")" + " = " + mata[0, columna - 1].ToString("f4") + "\n\n";

                double sum = 0;
                for (i = 0; i < fila; i++)
                {
                    sum = sum + mata[0, i] * mat1[i, fila];
                }
                cadena = cadena + Convert.ToString(sum) + " ≡ " + mata[0, columna - 1].ToString("f4") + "\n\n";
            }
            return (mat1);
        }
        /*
         * método que permite el calculo de determiantes de cualquier orden
         */
        private double determinante(double[,] mata, int tam, int metodo)
        {
            int i = 0, j = 0, k = 0, o = 0;
            double[] aux = new double[tam];
            double[] val_det = new double[tam];
            double[,] mat2 = new double[100, 101];
            for (i = 0; i < tam; i++)
            {
                for (j = 0; j < tam; j++)
                {
                    mat2[i, j] = mata[i, j];
                }
            }
            double au = 0, au1 = 0, det = 1;
            bool ordenar = true;
            o = 0;
            k = 0;
            while (ordenar == true)
            {
                for (i = 0; i < tam; i++)
                {
                    if (mat2[i, i] == 0)
                    {
                        ordenar = true;
                        //cadena = cadena + "\nSe reordena de manera conveniente\n";
                        for (j = 0; j < tam; j++)
                        {
                            if (j != i && mat2[j, i] != 0)
                            {
                                o++;
                                for (k = 0; k < tam; k++)
                                {
                                    aux[k] = mat2[j, k];
                                    mat2[j, k] = mat2[i, k];
                                    mat2[i, k] = aux[k];
                                }
                            }
                        }
                        //visualizar_Inversa(matInv, orden, orden * 2);
                        det = Math.Pow(-1, o);
                    }
                    else
                        ordenar = false;
                }
            }
            //for (i = 0; i < tam; i++)
            //{
            //    if (mat2[i, i] == 0)
            //    {
            //        ordenar = true;
            //    }
            //}
            //if (ordenar == true)
            //{
            //    o = 0;
            //    for (i = 0; i < tam; i++)
            //    {

            //        if (mat2[i, i] == 0 && i < tam - 1)
            //        {
            //            o++;
            //            for (j = 0; j < tam; j++)
            //            {
            //                aux[j] = mat2[i, j];
            //                mat2[i, j] = mat2[i + 1, j];
            //                mat2[i + 1, j] = aux[j];
            //            }
            //        }
            //        if (mat2[i, i] == 0 && i == tam - 1)
            //        {
            //            o++;
            //            for (j = 0; j < tam; j++)
            //            {
            //                aux[j] = mat2[i, j];
            //                mat2[i, j] = mat2[i - 1, j];
            //                mat2[i - 1, j] = aux[j];
            //            }
            //        }
            //    }
            //    det = Math.Pow(-1, o);
            //}
            for (i = 0; i < tam; i++)
            {
                au = mat2[i, i];
                val_det[i] = au;
                det = det * au;
                for (j = 0; j < tam; j++)
                {
                    mat2[i, j] = mat2[i, j] / au;
                }
                for (j = i + 1; j < tam; j++)
                {
                    if (i != j)
                    {
                        au1 = mat2[j, i];
                        for (k = 0; k < tam; k++)
                        {
                            mat2[j, k] = mat2[j, k] - au1 * mat2[i, k];
                        }
                    }
                }
            }
            return (det);
        }
        /*
         * evento que muestra la ventana de paso a paso
         */
        private void btn_pap_sis_Click(object sender, EventArgs e)
        {
            paso.Show();
            ((RichTextBox)paso.Controls["richTextBox_pap"]).Text = cadena;
            paso.Focus();
        }

        private void txt_fil_a_ValueChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        private void txt_col_a_ValueChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        private void txt_fil_b_ValueChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        private void txt_col_b_ValueChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        #region eventos random operaciones básicas
        private void btn_op_rma_Click(object sender, EventArgs e)
        {
            //fila
            int i = 0, j = 0;
            for (i = 0; i < na; i++)
            {
                //columna
                for (j = 0; j < ma; j++)
                {
                    /*
                     * llenado del datagridview, a diferencia de una matriz que se asigna de manera [i,j], 
                     * en el datagridview, es alrevés [j,i]
                     */
                    dgv_m_ob_a[j, i].Value = rnd.Next(-100, 101);
                }
            }

        }
        private void btn_op_rmb_Click(object sender, EventArgs e)
        {
            int i = 0, j = 0;
            for (i = 0; i < nb; i++)
            {
                for (j = 0; j < mb; j++)
                {
                    dgv_m_ob_b[j, i].Value = rnd.Next(-100, 101);
                }
            }
        }
        #endregion
        #region eventos operaciones básicas
        private void btn_op_sum_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0;
                double[,] res = new double[100, 100];
                double[,] a = new double[100, 100];
                double[,] b = new double[100, 100];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                dgv_m_ob_r.RowCount = nb;
                dgv_m_ob_r.ColumnCount = mb;
                for (i = 0; i < nb; i++)
                {
                    for (j = 0; j < mb; j++)
                    {
                        a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                        b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                        res[i, j] = a[i, j] + b[i, j];
                        dgv_m_ob_r[j, i].Value = res[i, j];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_op_res_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0;
                double[,] res = new double[100, 100];
                double[,] a = new double[100, 100];
                double[,] b = new double[100, 100];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                dgv_m_ob_r.RowCount = nb;
                dgv_m_ob_r.ColumnCount = mb;
                for (i = 0; i < nb; i++)
                {
                    for (j = 0; j < mb; j++)
                    {
                        a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                        b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                        res[i, j] = a[i, j] - b[i, j];
                        dgv_m_ob_r[j, i].Value = res[i, j];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_op_mul_axb_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0, k = 0;
                double[,] res = new double[na, mb];
                double[,] a = new double[na, ma];
                double[,] b = new double[nb, mb];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                dgv_m_ob_r.RowCount = na;
                dgv_m_ob_r.ColumnCount = mb;
                int af = na, ac = ma, bf = nb, bc = mb;
                for (i = 0; i < na; i++)
                {
                    for (j = 0; j < ma; j++)
                    {
                        a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                    }
                }
                for (i = 0; i < nb; i++)
                {
                    for (j = 0; j < mb; j++)
                    {
                        b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                    }
                }
                for (i = 0; i < af; i++)
                {
                    for (j = 0; j < bc; j++)
                    {
                        res[i, j] = 0;

                        for (k = 0; k < ac; k++)
                        {
                            res[i, j] = res[i, j] + a[i, k] * b[k, j];
                        }

                    }

                }
                for (i = 0; i < na; i++)
                {
                    for (j = 0; j < mb; j++)
                    {
                        dgv_m_ob_r[j, i].Value = res[i, j];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_op_mul_bxa_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0, k = 0;
                double[,] res = new double[nb, ma];
                double[,] b = new double[na, ma];
                double[,] a = new double[nb, mb];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                dgv_m_ob_r.RowCount = nb;
                dgv_m_ob_r.ColumnCount = ma;
                int af = nb, ac = mb, bf = na, bc = ma;
                for (i = 0; i < na; i++)
                {
                    for (j = 0; j < ma; j++)
                    {
                        b[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                    }
                }
                for (i = 0; i < nb; i++)
                {
                    for (j = 0; j < mb; j++)
                    {
                        a[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                    }
                }
                for (i = 0; i < af; i++)
                {
                    for (j = 0; j < bc; j++)
                    {
                        res[i, j] = 0;

                        for (k = 0; k < ac; k++)
                        {
                            res[i, j] = res[i, j] + a[i, k] * b[k, j];
                        }

                    }

                }
                for (i = 0; i < nb; i++)
                {
                    for (j = 0; j < ma; j++)
                    {
                        dgv_m_ob_r[j, i].Value = res[i, j];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_op_tras_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0;
                double[,] res = new double[100, 100];
                double[,] a = new double[100, 100];
                double[,] b = new double[100, 100];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                if (rdb_ma.Checked == true)
                {
                    dgv_m_ob_r.RowCount = ma;
                    dgv_m_ob_r.ColumnCount = na;
                    for (i = 0; i < na; i++)
                    {
                        for (j = 0; j < ma; j++)
                        {
                            a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                            dgv_m_ob_r[i, j].Value = a[i, j];
                        }
                    }
                }
                if (rdb_mb.Checked == true)
                {
                    dgv_m_ob_r.RowCount = mb;
                    dgv_m_ob_r.ColumnCount = nb;
                    for (i = 0; i < nb; i++)
                    {
                        for (j = 0; j < mb; j++)
                        {
                            b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                            dgv_m_ob_r[i, j].Value = b[i, j];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_op_mesc_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0;
                double[,] res = new double[100, 100];
                double[,] a = new double[100, 100];
                double[,] b = new double[100, 100];
                double escalar = 0;
                escalar = Convert.ToDouble(txt_op_escalar.Value);
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                if (rdb_ma.Checked == true)
                {
                    dgv_m_ob_r.RowCount = na;
                    dgv_m_ob_r.ColumnCount = ma;
                    for (i = 0; i < na; i++)
                    {
                        for (j = 0; j < ma; j++)
                        {
                            a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value) * escalar;
                            dgv_m_ob_r[j, i].Value = a[i, j];
                        }
                    }
                }
                if (rdb_mb.Checked == true)
                {
                    dgv_m_ob_r.RowCount = nb;
                    dgv_m_ob_r.ColumnCount = mb;
                    for (i = 0; i < nb; i++)
                    {
                        for (j = 0; j < mb; j++)
                        {
                            b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value) * escalar;
                            dgv_m_ob_r[j, i].Value = b[i, j];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(btn_randomInversa, "Genera valores aleatorios dentro de la matríz");
            this.toolTip1.SetToolTip(btn_cal_inv, "Calcular inversa y determinante de matríz");
            this.toolTip1.SetToolTip(btn_pap_inv, "Ver el paso a paso de la resolucion de la inversa, y del determinante");
            this.toolTip1.SetToolTip(dgv_inversa_a, "Matriz Ingreso");
            this.toolTip1.SetToolTip(dgv_inversa_b, "Matríz Respuesta");
            this.toolTip1.SetToolTip(txt_det, "Determinante de la matríz");
            this.toolTip1.SetToolTip(txt_ordenInversa, "Solo valores enteros mayores a 1");
            this.toolTip1.SetToolTip(groupBox10, "Operaciones que conllevan una sola matríz");
            this.toolTip1.SetToolTip(btn_op_rma, "Genera valores aleatorios dentro de la matríz A");
            this.toolTip1.SetToolTip(btn_op_rmb, "Genera valores aleatorios dentro de la matríz B");
            this.toolTip1.SetToolTip(btn_op_sum, "Calcular la suma matricial entre A y B");
            this.toolTip1.SetToolTip(btn_op_res, "Calcular la resta matricial entre A y B");
            this.toolTip1.SetToolTip(btn_op_mul_axb, "Calcular el producto matricial del orden AxB");
            this.toolTip1.SetToolTip(btn_op_mul_bxa, "Calcular el producto matricial del orden BxA");
            this.toolTip1.SetToolTip(btn_op_tras, "Calcular la transpuesta de la matríz seleccionada");
            this.toolTip1.SetToolTip(btn_op_mesc, "Calcular el producto entre la matríz seleccionada y un valor escalar");
            this.toolTip1.SetToolTip(btn_ran_sis, "Genera valores aleatorios dentro del sistema de ecuaciones");
            this.toolTip1.SetToolTip(btn_calc_sis, "Calcular el sistema de ecuaciones por el método seleccionado");
            this.toolTip1.SetToolTip(btn_pap_sis, "Ver el paso a paso de la resolucion del sistema de ecuaciones por el método seleccionado");
            this.toolTip1.SetToolTip(txt_fil_a, "Solo valores enteros mayores a 1");
            this.toolTip1.SetToolTip(txt_fil_b, "Solo valores enteros mayores a 1");
            this.toolTip1.SetToolTip(txt_col_a, "Solo valores enteros mayores a 1");
            this.toolTip1.SetToolTip(txt_col_b, "Solo valores enteros mayores a 1");  
        }

        private void txt_ordenInversa_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generar_tablas();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void txt_ordenInversa_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generar_tablas();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void txt_col_sis_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generarsistema();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void txt_col_sis_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generarsistema();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void txt_fil_sis_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generarsistema();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void txt_fil_sis_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //pictureBox1.Visible = true;
            generarsistema();
            //pictureBox1.Visible = false;
            Cursor.Current = Cursors.Default;
        }
       

        private void dgv_inversa_a_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dgv_inversa_a.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = dgv_inversa_a.CurrentCell.RowIndex;
                int col = dgv_inversa_a.CurrentCell.ColumnIndex;
                if (lines[lines.Length - 1] == "")
                {
                    txt_ordenInversa.Value = lines.Length - 1 + row;
                }
                else
                    txt_ordenInversa.Value = lines.Length + row;
                generar_tablas();
                foreach (string line in lines)
                {
                    if (row < dgv_inversa_a.RowCount && line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i < this.dgv_inversa_a.ColumnCount)
                            {
                                dgv_inversa_a[col + i, row].Value = Convert.ChangeType(cells[i], dgv_inversa_a[col + i, row].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void dgv_m_ob_a_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dgv_m_ob_a.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = dgv_m_ob_a.CurrentCell.RowIndex;
                int col = dgv_m_ob_a.CurrentCell.ColumnIndex;
                if (lines[lines.Length - 1] == "")
                {
                    txt_fil_a.Value = lines.Length - 1 + row;
                }
                else
                    txt_fil_a.Value = lines.Length + row;
            //txt_col_a.Value = 
                
                foreach (string line in lines)
                {
                    if (row < dgv_m_ob_a.RowCount && line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        txt_col_a.Value = cells.GetLength(0) + col;
                        generar_tablas();
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i < this.dgv_m_ob_a.ColumnCount)
                            {
                                dgv_m_ob_a[col + i, row].Value = Convert.ChangeType(cells[i], dgv_m_ob_a[col + i, row].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void dgv_m_ob_b_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dgv_m_ob_b.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = dgv_m_ob_b.CurrentCell.RowIndex;
                int col = dgv_m_ob_b.CurrentCell.ColumnIndex;
                if (lines[lines.Length - 1] == "")
                {
                    txt_fil_b.Value = lines.Length - 1 + row;
                }
                else
                    txt_fil_b.Value = lines.Length + row;
                //txt_col_a.Value = 

                foreach (string line in lines)
                {
                    if (row < dgv_m_ob_b.RowCount && line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        txt_col_b.Value = cells.GetLength(0)+col;
                        generar_tablas();
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i < this.dgv_m_ob_b.ColumnCount)
                            {
                                dgv_m_ob_b[col + i, row].Value = Convert.ChangeType(cells[i], dgv_m_ob_b[col + i, row].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void dgv_sis_a_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dgv_sis_a.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = dgv_sis_a.CurrentCell.RowIndex;
                int col = dgv_sis_a.CurrentCell.ColumnIndex;
                if (lines[lines.Length - 1] == "")
                {
                    txt_fil_sis.Value = lines.Length - 1+row;
                }
                else
                    txt_fil_sis.Value = lines.Length + row;
                //txt_col_a.Value = 

                foreach (string line in lines)
                {
                    if (row < dgv_sis_a.RowCount && line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        txt_col_sis.Value = cells.GetLength(0)-1+col;
                        generarsistema();
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i < this.dgv_sis_a.ColumnCount)
                            {
                                dgv_sis_a[col + i, row].Value = Convert.ChangeType(cells[i], dgv_sis_a[col + i, row].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        } 

        private void rdb_ma_CheckedChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        private void rdb_mb_CheckedChanged(object sender, EventArgs e)
        {
            generar_tablas();
        }

        private void btn_op_adjunta_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0, j = 0, k=0, l=0, m=0, n=0, indice = 0;
                double[,] res = new double[100, 100];
                double[,] a = new double[100, 100];
                double[,] b = new double[100, 100];
                double[,] aux1 = new double[100, 100];
                double[,] aux2 = new double[100, 100];
                dgv_m_ob_r.RowHeadersVisible = false;
                dgv_m_ob_r.ColumnHeadersVisible = false;
                if (rdb_ma.Checked == true)
                {
                    dgv_m_ob_r.RowCount = ma;
                    dgv_m_ob_r.ColumnCount = na;
                    for (i = 0; i < na; i++)
                    {
                        for (j = 0; j < ma; j++)
                        {
                            a[i, j] = Convert.ToDouble(dgv_m_ob_a[j, i].Value);
                        }
                    }
                    for (i=0;i<na;i++)
                    {
                        for(j=0;j<na;j++)
                        {
                            indice = 2 + i + j;
                            m=0;
                            for(k=0;k<na;k++)
                            {
                                n=0;
                                for(l=0;l<na;l++)
                                {
                                    if(k!=i && l!=j)
                                    {
                                        aux1[m,n] = a[k,l];
                                        n++;
                                    }
                                }
                                if (k != i)
                                {
                                    m++;
                                }
                            }
                            double det = determinante(aux1, na - 1, 0);
                            if (Double.IsNaN(det) || Double.IsInfinity(det)) //|| det == Double.N
                            {
                                det = 0;
                            }
                            //else if((Double.IsInfinity)det)
                            //{
                            //    det = 0;
                            //}
                            aux2[i, j] = Math.Pow((-1), indice) * det;
                            dgv_m_ob_r[i, j].Value = aux2[i, j].ToString("f4");
                        }
                    }
                }
                if (rdb_mb.Checked == true)
                {
                    dgv_m_ob_r.RowCount = mb;
                    dgv_m_ob_r.ColumnCount = nb;
                    for (i = 0; i < nb; i++)
                    {
                        for (j = 0; j < mb; j++)
                        {
                            b[i, j] = Convert.ToDouble(dgv_m_ob_b[j, i].Value);
                        }
                    }
                    for (i = 0; i < nb; i++)
                    {
                        for (j = 0; j < nb; j++)
                        {
                            indice = 2 + i + j;
                            m = 0;
                            for (k = 0; k < nb; k++)
                            {
                                n = 0;
                                for (l = 0; l < nb; l++)
                                {
                                    if (k != i && l != j)
                                    {
                                        aux1[m, n] = b[k, l];
                                        n++;
                                    }
                                }
                                if (k != i)
                                {
                                    m++;
                                }
                            }
                            double det = determinante(aux1, nb - 1, 0);
                            if (Double.IsNaN(det) || Double.IsInfinity(det)) //|| det == Double.N
                            {
                                det = 0;
                            }
                            //else if((Double.IsInfinity)det)
                            //{
                            //    det = 0;
                            //}
                            aux2[i, j] = Math.Pow((-1), indice) * det;
                            dgv_m_ob_r[i, j].Value = aux2[i, j].ToString("f4");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error\nIngrese Valores numéricos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

    }
}
