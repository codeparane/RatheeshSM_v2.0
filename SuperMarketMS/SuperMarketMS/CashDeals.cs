﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketMS
{
    public partial class CashDeals : Form
    {
        public CashDeals()
        {
            InitializeComponent();
        }
        DbConn dbconn = new DbConn();
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //load  data grid view
            dbconn.CloseConnection();
            dbconn.OpenConnection();
            string qGetStocks = "select * from sales where billDate >= '" + csSalesStart.Value.ToString("yyyy-MM-dd") + "' and billDate <= '" 
                + csSalesEd.Value.ToString("yyyy-MM-dd") + "' order by billid desc;";
            MySqlDataAdapter aGetStocks = new MySqlDataAdapter(qGetStocks, dbconn.connection);
            DataSet ds = new DataSet();
            aGetStocks.Fill(ds, "sto");
            dgvStocks.DataSource = ds.Tables["sto"];


            dbconn.CloseConnection();
            dbconn.OpenConnection();
            string qr_getProduct = "SELECT sum(amount) AS amount, sum(revenue)  AS revenue FROM sm.sales where billDate >= '" +
                csSalesStart.Value.ToString("yyyy-MM-dd") + "' and billDate <= '" + csSalesEd.Value.ToString("yyyy-MM-dd") + "' order by billid desc;" ;
            MySqlCommand cm_getProduct = new MySqlCommand(qr_getProduct, dbconn.connection);
            MySqlDataReader dr_getProduct = cm_getProduct.ExecuteReader();

            if (dr_getProduct.HasRows == true)
            {
                while (dr_getProduct.Read())
                {
                    if (dr_getProduct["amount"] != null && dr_getProduct["revenue"] != null && dr_getProduct["revenue"].ToString() != "" && dr_getProduct["revenue"].ToString() != "" )
                    {
                        csTotalRevenue.Text = decimal.Parse(dr_getProduct["revenue"].ToString()).ToString();
                        csTotalSales.Text = decimal.Parse(dr_getProduct["amount"].ToString()).ToString();
                    }
                }
                
            }
         
            //dbconn.CloseConnection();
            //dbconn.OpenConnection();
            //string qr_getProduct1 = "SELECT sum(amount) AS amount, sum(revenue)  AS revenue FROM sm.sales where billDate like '" +
            //    csSalesStart.Value.ToString("yyyy-MM") + "%';";
            //MySqlCommand cm_getProduct1 = new MySqlCommand(qr_getProduct1, dbconn.connection);
            //MySqlDataReader dr_getProduct1 = cm_getProduct1.ExecuteReader();

            //if (dr_getProduct1.HasRows == true)
            //{
            //    while (dr_getProduct1.Read())
            //    {
            //        if (dr_getProduct1["amount"] != null && dr_getProduct1["revenue"] != null && dr_getProduct1["revenue"].ToString() != "" && dr_getProduct1["revenue"].ToString() != "")
            //        {
            //            RevenueMonth.Text = decimal.Parse(dr_getProduct1["revenue"].ToString()).ToString();
            //            SalesMonth.Text = decimal.Parse(dr_getProduct1["amount"].ToString()).ToString();
            //        }
            //    }
            //}

        }

        private void CashDeals_Load(object sender, EventArgs e)
        {

        }

        private void csSalesEd_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1_ValueChanged("",e);
        }

        private void csTotalSales_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvStocks_RowEnter(object sender, DataGridViewCellEventArgs e)
        {


            if (dgvStocks.Rows[e.RowIndex].Cells[0].Value.ToString() != null && dgvStocks.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
            {

                billId.Text = dgvStocks.Rows[e.RowIndex].Cells[0].Value.ToString();
                billAmount.Text = dgvStocks.Rows[e.RowIndex].Cells[3].Value.ToString();
                csaAmount.Text = dgvStocks.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbconn.CloseConnection();
            dbconn.OpenConnection();
            string billNo = billId.Text;
            double Amount = double.Parse(billAmount.Text);
            double am = double.Parse(csaAmount.Text);

            //MessageBox.Show(delItemCode + delQty); 
            string qaddtobill = "update sales set revenue = "+ Amount +", amount = "+ am +"  where billid = '"+ billNo +"';";
            dbconn.CloseConnection();
            dbconn.OpenConnection();
            MySqlCommand caddtobill = new MySqlCommand(qaddtobill, dbconn.connection);
            int queryAffected = caddtobill.ExecuteNonQuery();

            dateTimePicker1_ValueChanged("", e);
            billId.Clear();
            billAmount.Clear();
            billAmount.Focus();
            csaAmount.Clear();
            MessageBox.Show("Amount Updated");
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbconn.CloseConnection();
            dbconn.OpenConnection();
            string billNo = dgvStocks.SelectedRows[0].Cells[0].Value.ToString();
            string Amount = dgvStocks.SelectedRows[0].Cells[3].Value.ToString();
            //MessageBox.Show(delItemCode + delQty); 
            string qaddtobill = "delete from sales where billid = '" + billNo + "';";
            dbconn.CloseConnection();
            dbconn.OpenConnection();
            MySqlCommand caddtobill = new MySqlCommand(qaddtobill, dbconn.connection);
            int queryAffected = caddtobill.ExecuteNonQuery();

            dateTimePicker1_ValueChanged("", e);
            billId.Clear();
            billAmount.Clear();
            billAmount.Focus();
            csaAmount.Clear();
        }
    }
}
