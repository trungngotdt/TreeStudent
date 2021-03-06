﻿using FizzWare.NBuilder;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tree;

namespace BSTreeStudent
{
    public class RunTree
    {
        
        public RunTree(ITree<Student> tree)
        {            
            this.tree = tree;
        }
        private ITree<Student> tree;

        private void MainMenu()
        {
            Console.WriteLine("1/ Traversal \n2/ Search \n3/ Insert \n4/ Remove \n5/ Get Predecessor and Get Successorr \n6/ Update " +
                "\n7/ Generate data \n8/ Exit \n Enter your choice :");
        }

        public void MainPanel()
        {
            int choice = 0;
            while (choice != 8)
            {
                MainMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            TraversalPanel();
                            break;
                        case 2:
                            SearchPanel();
                            break;
                        case 3:
                            InsertPanel();
                            break;
                        case 4:
                            RemovePanel();
                            break;
                        case 5:
                            PredeSuccessorPanel();
                            break;
                        case 6:
                            UpdatePanel();
                            break;
                        case 7:
                            GenerateData();
                            break;
                        case 8:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                    continue;
                }
            }
        }

        #region Generate Data
        private List<T> GetRandomData<T>(int size)
        {
            var list = Builder<T>.CreateListOfSize(size).Build().ToList();

            return list;
        }

        private List<Student> GetData(int size)
        {
            Random random = new Random();
            var list = GetRandomData<Student>(size);
            Parallel.ForEach(list, (item) =>
            {
                item.Id = item.Id - random.Next(-size, size) / 2 + random.Next(-size, size) + random.Next(-size, size) * 2;
                float mark = (random.Next(0, 10) / 1.0f) + 10.0f / (random.Next(0, 99) * 1.0f);
                item.AvgMark = float.Parse(String.Format("{0:0.00}", mark));
            });
            return list;
        }
        private void GenerateData()
        {
            Console.WriteLine("Enter size of data :");
            string input = Console.ReadLine();
            int size = 0;
            if (int.TryParse(input, out size))
            {
                Console.WriteLine("Please try again !");
                return;
            }
            try
            {
                var listStu = GetData(size);
                tree.AddRange(listStu.ToArray());
                Console.WriteLine("Everything OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        #endregion

        #region Traversal
        private void TraversalPanel()
        {
            int choice = 0;
            while (choice != 7)
            {
                TraversalMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            tree.LNR();
                            break;
                        case 2:
                            tree.LRN();
                            break;
                        case 3:
                            tree.NLR();
                            break;
                        case 4:
                            tree.RNL();
                            break;
                        case 5:
                            tree.NRL();
                            break;
                        case 6:
                            tree.RLN();
                            break;
                        case 7:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                    continue;
                }
            }
        }
        private void TraversalMenu()
        {
            Console.WriteLine("1/ LNR \n2/ LRN\n3/ NLR \n4/ RNL \n5/ NRL \n6/ RLN \n7/Exit \nEnter choice:");
        }
        #endregion

        #region Search

        private void SearchPanel()
        {
            int choice = 0;
            while (choice != 8)
            {
                string inp = null;
                SearchMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            SearchByID();
                            break;
                        case 2:
                            Console.WriteLine("Enter name of student :");
                            inp = Console.ReadLine();
                            SearchBy("Name", inp);
                            break;
                        case 3:
                            Console.WriteLine($"Enter birth day (format {DateTime.Now.ToString("MM/dd/yyyy")}):");
                            inp = Console.ReadLine();
                            SearchBy("BirthDay", inp, isDate: true);
                            break;
                        case 4:
                            Console.WriteLine("Enter mark of student :");
                            inp = Console.ReadLine();
                            SearchBy("AvgMark", inp, isNumber: true);
                            break;
                        case 5:
                            Console.WriteLine("Enter AccumulationCredit of student :");
                            inp = Console.ReadLine();
                            SearchBy("AccumulationCredit", inp, isNumber: true);
                            break;
                        case 6:
                            Console.WriteLine(tree.GetMax().Data.ToString());
                            break;
                        case 7:
                            Console.WriteLine(tree.GetMin().Data.ToString());
                            break;
                        case 8:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                    continue;
                }
            }
        }

        private void SearchMenu()
        {
            Console.WriteLine("1/ By ID \n2/ By Name \n3/ By Birth day \n4/ By Avg Mark \n5/ By Accumulation Credit " +
                "\n6/ Find Maximum \n7/ Find Minimum \n8/ Exit \nEnter choice :");
        }

        private void SearchByID()
        {
            Console.WriteLine("Enter ID of student :");
            int id = 0;
            string input = Console.ReadLine();
            if (int.TryParse(input, out id))
            {
                var student = tree.FindNode(new Node<Student>(new Student(id, null, DateTime.Now, 0, 0)));
                if (student==null)
                {
                    Console.WriteLine("Empty!");
                    return;
                }
                Console.WriteLine(student.Data.ToString());
                return;
            }
            Console.WriteLine("Please try again !");
            return;
        }

        private void SearchBy(string propertyName, string input, bool isNumber = false, bool isDate = false)
        {
            float num = 0;
            DateTime date = DateTime.Now;
            var checkDate = isDate ? DateTime.TryParse(input, out date) : true;
            var checkNumber = isNumber ? float.TryParse(input, out num) : true;
            if ((isNumber && !checkNumber) || (isDate && !checkDate))
            {
                Console.WriteLine("Please try again !");
                return;
            }
            input = isDate ? date.ToString() : input;
            input = isNumber ? num.ToString() : input;
            var list = tree.ToList().Where(p => p.GetType().GetProperty(propertyName).GetValue(p, null).ToString().Equals(input));
            if (list.Count() == 0)
            {
                Console.WriteLine("Empty");
                return;
            }
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }
        #endregion

        #region Insert
        private void InsertMenu()
        {
            Console.WriteLine("1/ Insert a student\n2/ Insert array student \n3/ Exit \n Enter your choice :");
        }

        private void InsertPanel()
        {
            int choice = 0;
            while (choice != 3)
            {
                string inp = null;
                InsertMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            InsertAElement();
                            break;
                        case 2:
                            InsertAListStudent();
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                    continue;
                }
            }
        }


        private void InsertAListStudent()
        {
            var listStudent = GetDataFromExcel();
            try
            {
                foreach (var item in listStudent)
                {
                    tree.Insert(new Node<Student>(item));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        private void InsertAElement()
        {
            Console.WriteLine("Enter ID of student :");
            string StrId = Console.ReadLine();
            var checkId = int.TryParse(StrId, out int id);
            if (!checkId)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine("Enter name of student :");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Avg Mark of student :");
            string Avg = Console.ReadLine();
            var checkMark = float.TryParse(Avg, out float avgMark);
            if (!checkMark)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine("Enter Accumulation Credit of student :");
            string accCredit = Console.ReadLine();
            //int.Parse( Console.ReadLine());
            var checkCredit = int.TryParse(accCredit, out int accumulationCredit);
            if (!checkCredit)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine($"Enter Birth Day of student (format {DateTime.Now.ToString("MM / dd / yyyy")}):");
            string birthDay = Console.ReadLine();
            var checkDate = DateTime.TryParse(birthDay, out DateTime date);
            if (!checkDate)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            var student = new Student(id, name, date, avgMark, accumulationCredit);
            tree.Insert(new Node<Student>(student));
        }
        #endregion

        #region Remove

        private void RemoveMenu()
        {
            Console.WriteLine("1/ Remove a student\n2/ Remove array student\n3/ Exit \n Enter your choice :");
        }

        private void RemovePanel()
        {
            int choice = 0;
            while (choice != 3)
            {
                string inp = null;
                RemoveMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RemoveAElement();
                            break;
                        case 2:
                            RemoveAList();
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                    continue;
                }
            }

        }

        public void RemoveAList()
        {
            var listStudent = GetDataFromExcel();
            if (listStudent==null)
            {
                Console.WriteLine("Empty!");
            }
            foreach (var student in listStudent)
            {
                var resultDelete = tree.Remove(new Node<Student>(student));
                var result = !tree.Contains(new Node<Student>(student));
                if (!result)
                {
                    string mess = tree.FindNode(student) != null ? "Can't remove" : $"Can't find {student.ToString()}";
                    Console.WriteLine(mess);
                }
                else
                {
                    Console.WriteLine("Complete");
                }
            }
        }

        private void RemoveAElement()
        {
            Console.WriteLine("Enter ID of student :");
            string StrId = Console.ReadLine();
            var checkId = int.TryParse(StrId, out int id);
            if (!checkId)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine("Enter name of student :");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Avg Mark of student :");
            string Avg = Console.ReadLine();
            var checkMark = float.TryParse(Avg, out float avgMark);
            if (!checkMark)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine("Enter Accumulation Credit of student :");
            string accCredit = Console.ReadLine();
            var checkCredit = int.TryParse(accCredit, out int accumulationCredit);
            if (!checkCredit)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            Console.WriteLine($"Enter Birth Day of student (format {DateTime.Now.ToString("MM / dd / yyyy")}):");
            string birthDay = Console.ReadLine();
            var checkDate = DateTime.TryParse(birthDay, out DateTime date);
            if (!checkDate)
            {
                Console.WriteLine("Please try again !");
                return;
            }
            var student = new Student(id, name, date, avgMark, accumulationCredit);
            if (!tree.Contains( student))
            {
                Console.WriteLine("Empty!");
                return;
            }
            var check= tree.Remove(new Node<Student>(student));
            var mess = check ? "OK" : "Can't delete";
            Console.WriteLine(mess);
        }

        #endregion

        #region GetPredecessorAndGetSuccessor

        private void PredeSuccessorMenu()
        {
            Console.WriteLine("1/ Get Predecessor \n2/ GetSuccessor \n3/ Exit \n Enter your choice :");
        }

        private void PredeSuccessorPanel()
        {
            int choice = 0;
            while (choice != 3)
            {
                PredeSuccessorMenu();
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine((tree.Predecessor() as Node<Student>).Data.ToString());
                            break;
                        case 2:
                            Console.WriteLine((tree.Successor() as Node<Student>).Data.ToString());
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Please try again !");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again !");
                }
            }
        }

        #endregion

        #region Update

        private void UpdatePanel()
        {
            int choice = 0;
            while (choice != 8)
            {
                Node<Student> node = null;
                string inp = null;
                UpdateMenu();
                string input = Console.ReadLine();
                while (node == null)
                {
                    Console.WriteLine("Out : Exit");
                    string Out= Console.ReadLine();
                    if (Out.Equals("Exit"))
                    {
                        return;
                    }
                    node = FindStudent();
                }
                if (node==null)
                {
                    return;
                }
                //Console.WriteLine("Old data of student");
                //Console.WriteLine(node.Data.ToString());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter name of student :");
                        inp = Console.ReadLine();
                        UpdateWith("Name", inp, node);
                        break;
                    case 2:
                        Console.WriteLine($"Enter birth day (format {DateTime.Now.ToString("MM/dd/yyyy")}):");
                        inp = Console.ReadLine();
                        UpdateWith("BirthDay", inp, node);
                        break;
                    case 3:
                        Console.WriteLine("Enter mark of student :");
                        inp = Console.ReadLine();
                        UpdateWith("AvgMark", inp, node);
                        break;
                    case 4:
                        Console.WriteLine("Enter AccumulationCredit of student :");
                        inp = Console.ReadLine();
                        UpdateWith("AccumulationCredit", inp, node);
                        break;
                    default:
                        Console.WriteLine("Please try again !");
                        break;
                }
            }
        }

        private void UpdateMenu()
        {
            Console.WriteLine("1/ Update Name \n2/ Birth Day \n3/ Avg Mark \n4/ Accumulation Credit \n5/ Exit \n Enter your choice :");
        }

        private Node<Student> FindStudent()
        {
            Console.WriteLine("Enter ID of student :");
            int id = 0;
            string input = Console.ReadLine();
            if (int.TryParse(input, out id))
            {
                var student = tree.FindNode(new Node<Student>(new Student(id, null, DateTime.Now, 0, 0)));
                if (student==null)
                {
                    Console.WriteLine("Empty!");
                    return null;
                }
                Console.WriteLine(student.Data.ToString());
                return student;
            }
            Console.WriteLine("Please try again !");
            return null;

        }

        private void UpdateWith(string propertyName, string data, Node<Student> node)
        {
            object value;
            if (propertyName.Equals("Name"))
            {
                value = data;
            }
            else if (propertyName.Equals("AvgMark"))
            {
                if (!float.TryParse(data, out float avg))
                {
                    Console.WriteLine("Please try again !");
                    return;
                }
                value = avg;
            }
            else if (propertyName.Equals("AccumulationCredit"))
            {
                if (!int.TryParse(data, out int credit))
                {
                    Console.WriteLine("Please try again !");
                    return;
                }
                value = credit;
            }
            else if (propertyName.Equals("BirthDay"))
            {
                if (!DateTime.TryParse(data, out DateTime date))
                {
                    Console.WriteLine("Please try again !");
                    return;
                }
                value = date;
            }
            else
            {
                Console.WriteLine("Please try again !");
                return;
            }
            var propInfo = node.Data.GetType().GetProperty(propertyName);
            propInfo.SetValue(node.Data, Convert.ChangeType(value, propInfo.PropertyType), null);//.GetValue(node.Data, null)="";
        }
        #endregion

        #region GetDataFromExcel
        public List<Student> GetDataFromExcel()
        {
            List<Student> list = new List<Student>();
            Console.WriteLine("Path to excel file :");
            string pathToExcelFile = Console.ReadLine();
            string ext = Path.GetExtension(pathToExcelFile);
            if (ext.ToLower().Equals(".xls") || ext.ToLower().Equals(".xlsx"))
            {
                Console.WriteLine("Sheet Name :");
                string sheetName = Console.ReadLine();
                var excelFile = new ExcelQueryFactory(pathToExcelFile);
                var dataExcel = from a in excelFile.Worksheet(sheetName) select a;
                foreach (var a in dataExcel)
                {
                    try
                    {
                        string info = $"ID:{a[0].Value} Name: {a[1].Value}; AvgMark: {a[2].Value}; AccumulationCredit: {a[3].Value}; BirthDay{a[4].Value}";
                        Console.WriteLine(info);
                        var ID = a[0].Cast<int>();
                        var Name = a[1].Value.ToString();
                        var AvgMark = a[2].Cast<float>();
                        var AccumulationCredit = a[3].Cast<int>();
                        var BirthDay = a[4].Cast<DateTime>();
                        Student student = new Student(ID, Name, BirthDay, AvgMark, AccumulationCredit);
                        //tree.Insert(new Node<Student>(student));
                        list.Add(student);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Something wrong with data !Please try again !");
                    }
                }
                return list;
            }
            else
            {
                Console.WriteLine("This isn't a excel file!");
                return null;
            }

        }
        #endregion
    }
}
