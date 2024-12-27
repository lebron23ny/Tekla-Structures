using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;
using TSF = Tekla.Structures.Filtering;
using TSFC = Tekla.Structures.Filtering.Categories;
using System.Collections;
using Tekla.Structures.Filtering;
using Tekla.Structures.Filtering.Categories;
using Tekla.Structures.Model;

namespace TeklaApi_Work_with_filter
{
    internal class Program
    {
        static void Main(string[] args)
        {





                //ReadUserProperty();
                //SetUserProperty();
                //SelectPartBySaveFilterInTekla();
                //CreateFilterExpressionCollection();
                //CreateFilterExpColl_by_Template();
                //CreateFilterExpColl_by_Template2();
            }
        



        /// <summary>
        /// Получение TSM.ModelObjectEnumerator по сохраненному фильтру "standard12" в Текле и выделение в моделе 
        /// </summary>
        static void SelectPartBySaveFilterInTekla()
        {
            TSM.Model model = new TSM.Model();
            if (model.GetConnectionStatus())
            {
                ArrayList arrayList = new ArrayList();
                TSM.ModelObjectEnumerator modelObjectEnumerator = model.GetModelObjectSelector().GetObjectsByFilterName("standard12");
                while(modelObjectEnumerator.MoveNext())
                {
                    TSM.ModelObject modelObject = modelObjectEnumerator.Current;
                    arrayList.Add(modelObject);
                }


                TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
                selector.Select(arrayList);
            }
            else
            {
                Console.WriteLine("Подключиться не удалось");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Создание фильтра typeObject == Part && profile=="I20Ш1" && material=="C245"
        /// </summary>
        static void CreateFilterExpressionCollection()
        {
            TSM.Model model = new TSM.Model();
            if (model.GetConnectionStatus())
            {
                ///Создаем чать фильтра typeObject = Part
                TSFC.ObjectFilterExpressions.Type objectType = new TSFC.ObjectFilterExpressions.Type();
                TSF.NumericConstantFilterExpression type = new NumericConstantFilterExpression(Tekla.Structures.TeklaStructuresDatabaseTypeEnum.PART);
                TSF.BinaryFilterExpression AA = new BinaryFilterExpression(objectType, NumericOperatorType.IS_EQUAL, type);


                ///Создаем часть фильтра profile=="I20Ш1"
                TSFC.PartFilterExpressions.Profile profileExpression = new TSFC.PartFilterExpressions.Profile();
                TSF.StringConstantFilterExpression partProfile = new StringConstantFilterExpression(ConvertToCP1251("I20Ш1"));
                TSF.BinaryFilterExpression A = new BinaryFilterExpression(profileExpression, StringOperatorType.IS_EQUAL, partProfile);


                ///Создаем часть фильтра material=="C245"
                TSFC.PartFilterExpressions.Material materailExpression = new TSFC.PartFilterExpressions.Material();
                TSF.StringConstantFilterExpression partMaterial = new StringConstantFilterExpression(ConvertToCP1251("C245"));
                TSF.BinaryFilterExpression B = new BinaryFilterExpression(materailExpression, StringOperatorType.IS_EQUAL, partMaterial);


                //Создаем фильтр A && B
                TSF.BinaryFilterExpressionCollection expressionCollection = new BinaryFilterExpressionCollection();
                expressionCollection.Add(new BinaryFilterExpressionItem(AA, BinaryFilterOperatorType.BOOLEAN_AND));
                expressionCollection.Add(new BinaryFilterExpressionItem(A, BinaryFilterOperatorType.BOOLEAN_AND));
                expressionCollection.Add(new BinaryFilterExpressionItem(B, BinaryFilterOperatorType.BOOLEAN_AND));

                //Выбираем объекты из модели соответсвующие списку
                ArrayList arrayList = new ArrayList();

                TSM.ModelObjectEnumerator modelObjectEnumerator = model.GetModelObjectSelector().GetObjectsByFilter(expressionCollection);


                var d = expressionCollection.ToString();
                while (modelObjectEnumerator.MoveNext())
                {
                    TSM.ModelObject modelObject = modelObjectEnumerator.Current;
                    arrayList.Add(modelObject);
                }

                TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
                selector.Select(arrayList);

            }
            else
            {
                Console.WriteLine("Подключиться не удалось");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Создание фильтра guid = ******* выбор деталей соответсвующего guid
        /// </summary>
        static void selectByGuid()
        {
            TSM.Model model = new TSM.Model();
            if (model.GetConnectionStatus())
            {
                
                TSFC.ObjectFilterExpressions.Guid filterGuid = new TSFC.ObjectFilterExpressions.Guid();
                TSF.StringConstantFilterExpression guid = new StringConstantFilterExpression("76650888-3d97-4e71-a6eb-36f33593481a");
                TSF.BinaryFilterExpression A = new BinaryFilterExpression(filterGuid, StringOperatorType.IS_EQUAL, guid);


                //Выбираем объекты из модели соответсвующие списку
                ArrayList arrayList = new ArrayList();
                TSM.ModelObjectEnumerator modelObjectEnumerator = model.GetModelObjectSelector().GetObjectsByFilter(A);

                while (modelObjectEnumerator.MoveNext())
                {
                    TSM.ModelObject modelObject = modelObjectEnumerator.Current;
                    arrayList.Add(modelObject);
                }

                TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
                selector.Select(arrayList);

            }
            else
            {
                Console.WriteLine("Подключиться не удалось");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Создание фильтра typeObject == Part && template(PROFILE_TYPE) != "B"
        /// </summary>
        static void CreateFilterExpColl_by_Template()
        {
            TSM.Model model = new TSM.Model();

            ///Создаем часть фильтра template(PROFILE_TYPE) != "B"
            TSFC.TemplateFilterExpressions.CustomString customString = new TemplateFilterExpressions.CustomString("PROFILE_TYPE");
            TSF.StringConstantFilterExpression gost_name = new StringConstantFilterExpression("B");
            TSF.BinaryFilterExpression A = new BinaryFilterExpression(customString, StringOperatorType.IS_NOT_EQUAL, gost_name);

            ///Создаем чать фильтра typeObject = Part
            TSFC.ObjectFilterExpressions.Type objectType = new TSFC.ObjectFilterExpressions.Type();
            TSF.NumericConstantFilterExpression type = new NumericConstantFilterExpression(Tekla.Structures.TeklaStructuresDatabaseTypeEnum.PART);
            TSF.BinaryFilterExpression AA = new BinaryFilterExpression(objectType, NumericOperatorType.IS_EQUAL, type);


            TSF.BinaryFilterExpressionCollection expressionCollection = new BinaryFilterExpressionCollection();
            expressionCollection.Add(new BinaryFilterExpressionItem(AA, BinaryFilterOperatorType.BOOLEAN_AND));
            expressionCollection.Add(new BinaryFilterExpressionItem(A, BinaryFilterOperatorType.BOOLEAN_AND));

            //Выбираем объекты из модели соответсвующие списку
            ArrayList arrayList = new ArrayList();
            TSM.ModelObjectEnumerator modelObjectEnumerator = model.GetModelObjectSelector().GetObjectsByFilter(expressionCollection);

            while (modelObjectEnumerator.MoveNext())
            {
                TSM.ModelObject modelObject = modelObjectEnumerator.Current;
                arrayList.Add(modelObject);
            }

            TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
            selector.Select(arrayList);
        }


        /// <summary>
        /// Создание фильтра typeObject == Part && template(PROFILE.TPL_NAME_ENG) == "30K1"
        /// </summary>
        static void CreateFilterExpColl_by_Template2()
        {
            TSM.Model model = new TSM.Model();
            TSFC.TemplateFilterExpressions.CustomString customString = new TemplateFilterExpressions.CustomString("PROFILE.TPL_NAME_ENG");
            TSF.StringConstantFilterExpression gost_name = new StringConstantFilterExpression("35H2");
            TSF.BinaryFilterExpression A = new BinaryFilterExpression(customString, StringOperatorType.IS_EQUAL, gost_name);

            ///Создаем чать фильтра typeObject = Part
            TSFC.ObjectFilterExpressions.Type objectType = new TSFC.ObjectFilterExpressions.Type();
            TSF.NumericConstantFilterExpression type = new NumericConstantFilterExpression(Tekla.Structures.TeklaStructuresDatabaseTypeEnum.PART);
            TSF.BinaryFilterExpression AA = new BinaryFilterExpression(objectType, NumericOperatorType.IS_EQUAL, type);


            TSF.BinaryFilterExpressionCollection expressionCollection = new BinaryFilterExpressionCollection();
            expressionCollection.Add(new BinaryFilterExpressionItem(AA, BinaryFilterOperatorType.BOOLEAN_AND));
            expressionCollection.Add(new BinaryFilterExpressionItem(A, BinaryFilterOperatorType.BOOLEAN_AND));

            //Выбираем объекты из модели соответсвующие списку
            ArrayList arrayList = new ArrayList();
            TSM.ModelObjectEnumerator modelObjectEnumerator = model.GetModelObjectSelector().GetObjectsByFilter(expressionCollection);

            while (modelObjectEnumerator.MoveNext())
            {
                TSM.ModelObject modelObject = modelObjectEnumerator.Current;
                arrayList.Add(modelObject);
            }

            TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
            selector.Select(arrayList);
        }


        // Creates the following expression:
        // ((PartName != BEAM AND PartName != BEAM1) AND
        // (PartName != BEAM2 AND PartName != BEAM3) AND
        // (PartName != BEAM4 AND PartName != BEAM5))
        public static FilterExpression CreateBinaryFilterExpressionCollection5()
        {
            PartFilterExpressions.Name PartName = new PartFilterExpressions.Name();
            StringConstantFilterExpression BeamName = new StringConstantFilterExpression("BEAM");
            StringConstantFilterExpression BeamName1 = new StringConstantFilterExpression("BEAM1");
            StringConstantFilterExpression BeamName2 = new StringConstantFilterExpression("BEAM2");
            StringConstantFilterExpression BeamName3 = new StringConstantFilterExpression("BEAM3");
            StringConstantFilterExpression BeamName4 = new StringConstantFilterExpression("BEAM4");
            StringConstantFilterExpression BeamName5 = new StringConstantFilterExpression("BEAM5");

            BinaryFilterExpression A = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName);
            BinaryFilterExpression B = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName1);

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection.Add(new BinaryFilterExpressionItem(A, BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection.Add(new BinaryFilterExpressionItem(B));

            BinaryFilterExpression C = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName2);
            BinaryFilterExpression D = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName3);

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection1 = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection1.Add(new BinaryFilterExpressionItem(C, BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection1.Add(new BinaryFilterExpressionItem(D));

            BinaryFilterExpression E = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName4);
            BinaryFilterExpression F = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName5);

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection2 = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection2.Add(new BinaryFilterExpressionItem(E, BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection2.Add(new BinaryFilterExpressionItem(F));

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection3 = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection3.Add(new BinaryFilterExpressionItem(BinaryFilterExpressionCollection,
                BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection3.Add(new BinaryFilterExpressionItem(BinaryFilterExpressionCollection1,
                BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection3.Add(new BinaryFilterExpressionItem(BinaryFilterExpressionCollection2));

            return BinaryFilterExpressionCollection3;
        }



        // Creates the following expression:
        // (PartName != BEAM AND (PartName != BEAM2 OR PartName != BEAM3) AND PartName != BEAM1)
        public static FilterExpression CreateBinaryFilterExpressionCollection3()
        {
            PartFilterExpressions.Name PartName = new PartFilterExpressions.Name();
            StringConstantFilterExpression BeamName = new StringConstantFilterExpression("BEAM");
            StringConstantFilterExpression BeamName1 = new StringConstantFilterExpression("BEAM1");
            StringConstantFilterExpression BeamName2 = new StringConstantFilterExpression("BEAM2");
            StringConstantFilterExpression BeamName3 = new StringConstantFilterExpression("BEAM3");

            BinaryFilterExpression A = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName);
            BinaryFilterExpression B = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName1);
            BinaryFilterExpression C = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName2);
            BinaryFilterExpression D = new BinaryFilterExpression(PartName, StringOperatorType.IS_NOT_EQUAL, BeamName3);

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection.Add(new BinaryFilterExpressionItem(C, BinaryFilterOperatorType.BOOLEAN_OR));
            BinaryFilterExpressionCollection.Add(new BinaryFilterExpressionItem(D));

            BinaryFilterExpressionCollection BinaryFilterExpressionCollection1 = new BinaryFilterExpressionCollection();
            BinaryFilterExpressionCollection1.Add(new BinaryFilterExpressionItem(A, BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection1.Add(new BinaryFilterExpressionItem(BinaryFilterExpressionCollection,
                BinaryFilterOperatorType.BOOLEAN_AND));
            BinaryFilterExpressionCollection1.Add(new BinaryFilterExpressionItem(B));

            return BinaryFilterExpressionCollection1;
        }


        
        private static string ConvertToCP1251(string text)
        {

            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(text);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }
    }
}
