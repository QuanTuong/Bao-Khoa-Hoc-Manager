﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace DoAn1_QuanLy_BaoKhoaHoc.DB
{
    class DBMain
    {
        string ConnStr = @"Data Source=DESKTOP-TM29P3C\SQLEXPRESS;Initial Catalog=BaoCaoKhoaHoc;Integrated Security=True";
        SqlConnection conn = null;
        SqlCommand comm = null;
        SqlDataAdapter da = null;
        DataTable dt;
        SqlCommand cmd;
        public DBMain()
        {
            conn = new SqlConnection(ConnStr);
            comm = conn.CreateCommand();
        }
        public DataSet ExecuteQueryDataSet(string strSQL, CommandType ct)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            da = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error)
        {
            bool f = false;
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            try
            {
                comm.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
        public DataTable CheckLogin(string strSQL, CommandType ct, ref string err)
        { 
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            try
            {
                dt = new DataTable();
                cmd = new SqlCommand(strSQL, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                err = ex.Message;
            }
            finally
            {
                conn.Close();
                
            }
            return dt;
        }
        public bool GetLuong(string sql, ref string LuongCB, ref string HeSoLuong)
        {
            SqlDataAdapter DataAdapter = new SqlDataAdapter(sql, conn);

            DataSet dataSet = new DataSet();
            DataAdapter.Fill(dataSet, "Luong");
            DataTable dataTable = dataSet.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                LuongCB = dataRow["LuongCB"].ToString();
                HeSoLuong = dataRow["HeSoLuong"].ToString();
            }
            return true;
        }

        public bool Check(string sql, CommandType ct, ref string error)
        {
            bool f = false;
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = sql;
            comm.CommandType = ct;
            try
            {
                int k = Int32.Parse(comm.ExecuteScalar().ToString());
                if (k > 0)
                {
                    f = true;
                }
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
        //Thực thi nhanh nếu chỉ cần lấy dữ liệu trả ra
        public DataTable truyvan(string sql)
        {
            conn.Open();
            dt = new DataTable();
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        //Lấy giá trị trả ra cho Combobox
        public bool LoadCombobox(ComboBox cbb, string display, string value, string sql)
        {
            if(conn.State== ConnectionState.Open)
            {
                conn.Close();
            }
            dt = new DataTable();
            dt = truyvan(sql);
            cbb.DataSource = dt;
            cbb.DisplayMember = display;
            cbb.ValueMember = value;
            return true;
        }
    }
}
