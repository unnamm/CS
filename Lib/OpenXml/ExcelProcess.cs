using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace Lib.OpenXml
{
    /// <summary>
    /// DocumentFormat.OpenXml
    /// lack explanation
    /// will add it later
    /// </summary>
    internal class ExcelProcess
    {
        string[]? Sheet1Headers;
        string[][]? Sheet1Data;

        public void MakeFile(string filePath, string[][] rowDatas)
        {
            Sheet1Headers = rowDatas[0];

            List<string[]> listlist = [];
            foreach (var v in rowDatas)
            {
                List<string> list = [];

                foreach (var k in v)
                {
                    if (k == null)
                    {
                        list.Add("-");
                    }
                    else
                    {
                        list.Add(k);
                    }
                }
                listlist.Add([.. list]);
            }

            Sheet1Data = [.. listlist];

            using SpreadsheetDocument excel = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookpart = excel.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            WorksheetPart worksheetPart1 = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart1.Worksheet = new Worksheet();
            SheetData data1 = createShee1Data();
            Columns columns1 = autoSizeCells(data1);
            worksheetPart1.Worksheet.Append(columns1);
            worksheetPart1.Worksheet.Append(data1);

            //WorksheetPart worksheetPart2 = workbookpart.AddNewPart<WorksheetPart>();
            //worksheetPart2.Worksheet = new Worksheet();
            //SheetData data2 = CreateShee2Data();
            //Columns columns2 = AutoSizeCells(data2);
            //worksheetPart2.Worksheet.Append(columns2);
            //worksheetPart2.Worksheet.Append(data2);

            var stylesPart = workbookpart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = createStyleSheet();
            stylesPart.Stylesheet.Save();

            Sheets sheets = excel.WorkbookPart!.Workbook.AppendChild(new Sheets());
            sheets.Append(new Sheet()
            {
                Id = excel.WorkbookPart.GetIdOfPart(worksheetPart1),
                SheetId = 1,
                Name = "Sheet 1"
            });
            //sheets.Append(new Sheet()
            //{
            //    Id = excel.WorkbookPart.GetIdOfPart(worksheetPart2),
            //    SheetId = 2,
            //    Name = "Sheet 2"
            //});

            //Save & close
            workbookpart.Workbook.Save();
        }

        private static void insertTextCell(Row row, string content, int cellIndex, UInt32Value styleIndex)
        {
            row.InsertAt<Cell>(new Cell() { DataType = CellValues.InlineString, InlineString = new InlineString() { Text = new Text(content) }, StyleIndex = styleIndex }, cellIndex);
        }

        //void InsertTransitCell(Row row, string content, int cellIndex)
        //{
        //    row.InsertAt<Cell>(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(content), StyleIndex = 7 }, cellIndex);
        //}

        //void InsertNumberCell(Row row, string content, int cellIndex)
        //{
        //    row.InsertAt<Cell>(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(content), StyleIndex = 5 }, cellIndex);
        //}

        //void InsertDateCell(Row row, string content, int cellIndex)
        //{
        //    row.InsertAt<Cell>(new Cell() { DataType = CellValues.Date, CellValue = new CellValue(content), StyleIndex = 1 }, cellIndex);
        //}

        private SheetData createShee1Data()
        {
            if (Sheet1Headers == null || Sheet1Data == null)
                throw new Exception("null error");

            SheetData data = new();
            var row = new Row();
            int rowId = 0;

            for (int i = 0; i < Sheet1Headers.Length; i++)
            {
                row.InsertAt(new Cell()
                {
                    DataType = CellValues.InlineString,
                    InlineString = new InlineString() { Text = new Text(Sheet1Headers[i]) },
                    StyleIndex = 8
                }, i);
            }

            data.InsertAt(row, rowId++);

            for (int i = 0; i < Sheet1Data.Length; i++)
            {
                row = new Row();

                for (int k = 0; k < Sheet1Data[i].Length; k++)
                {
                    insertTextCell(row, Sheet1Data[i][k], k, 9);
                }

                data.InsertAt(row, rowId++);
            }

            return data;
        }

        private static Dictionary<int, int> getMaxCharacterWidth(SheetData sheetData)
        {
            //iterate over all cells getting a max char value for each column
            Dictionary<int, int> maxColWidth = [];
            var rows = sheetData.Elements<Row>();
            UInt32[] numberStyles = [5, 6, 7, 8]; //styles that will add extra chars
            UInt32[] boldStyles = [1, 2, 3, 4, 6, 7, 8]; //styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                //using cell index as my column
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? cell.InnerText : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        //add 3 for '.00' 
                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        //add an extra char for bold - not 100% acurate but good enough for what i need.
                        cellTextLength += 1;
                    }

                    if (maxColWidth.TryGetValue(i, out int value))
                    {
                        var current = value;
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        private static Columns autoSizeCells(SheetData sheetData)
        {
            var maxColWidth = getMaxCharacterWidth(sheetData);

            Columns columns = new Columns();
            //this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                //width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;
                Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                columns.Append(col);
            }

            return columns;
        }

        private static ForegroundColor translateForeground(System.Drawing.Color fillColor)
        {
            return new ForegroundColor()
            {
                Rgb = new HexBinaryValue()
                {
                    Value =
                              System.Drawing.ColorTranslator.ToHtml(
                              System.Drawing.Color.FromArgb(
                                  fillColor.A,
                                  fillColor.R,
                                  fillColor.G,
                                  fillColor.B)).Replace("#", "")
                }
            };
        }

        private static Stylesheet createStyleSheet()
        {
            Stylesheet stylesheet = new Stylesheet();
            #region Number format
            uint DATETIME_FORMAT = 164;
            uint DIGITS4_FORMAT = 165;
            var numberingFormats = new NumberingFormats();
            numberingFormats.Append(new NumberingFormat // Datetime format
            {
                NumberFormatId = UInt32Value.FromUInt32(DATETIME_FORMAT),
                FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss")
            });
            numberingFormats.Append(new NumberingFormat // four digits format
            {
                NumberFormatId = UInt32Value.FromUInt32(DIGITS4_FORMAT),
                FormatCode = StringValue.FromString("0000")
            });
            numberingFormats.Count = UInt32Value.FromUInt32((uint)numberingFormats.ChildElements.Count);
            #endregion

            #region Fonts
            var fonts = new Fonts();
            fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()  // Font index 0 - default
            {
                FontName = new FontName { Val = StringValue.FromString("Calibri") },
                FontSize = new FontSize { Val = DoubleValue.FromDouble(11) }
            });
            fonts.Append(new DocumentFormat.OpenXml.Spreadsheet.Font()  // Font index 1
            {
                FontName = new FontName { Val = StringValue.FromString("Arial") },
                FontSize = new FontSize { Val = DoubleValue.FromDouble(11) },
                Bold = new Bold()
            });
            fonts.Count = UInt32Value.FromUInt32((uint)fonts.ChildElements.Count);
            #endregion

            #region Fills
            var fills = new Fills();
            fills.Append(new Fill() // Fill index 0
            {
                PatternFill = new PatternFill { PatternType = PatternValues.None }
            });
            fills.Append(new Fill() // Fill index 1
            {
                PatternFill = new PatternFill { PatternType = PatternValues.Gray125 }
            });
            fills.Append(new Fill() // Fill index 2
            {
                PatternFill = new PatternFill
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = translateForeground(System.Drawing.Color.LightBlue),
                    BackgroundColor = new BackgroundColor { Rgb = translateForeground(System.Drawing.Color.LightBlue).Rgb }
                }
            });
            fills.Append(new Fill() // Fill index 3
            {
                PatternFill = new PatternFill
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = translateForeground(System.Drawing.Color.LightSkyBlue),
                    BackgroundColor = new BackgroundColor { Rgb = translateForeground(System.Drawing.Color.Black).Rgb }
                }
            });
            fills.Append(new Fill() // Fill index 4
            {
                PatternFill = new PatternFill
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = translateForeground(System.Drawing.Color.OrangeRed),
                    BackgroundColor = new BackgroundColor { Rgb = translateForeground(System.Drawing.Color.Black).Rgb },
                }
            });
            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
            #endregion

            #region Borders
            var borders = new Borders();
            borders.Append(new Border   // Border index 0: no border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            });
            borders.Append(new Border    //Boarder Index 1: All
            {
                LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
                RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
                TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
                BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
                DiagonalBorder = new DiagonalBorder()
            });
            borders.Append(new Border   // Boarder Index 2: Top and Bottom
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
                BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
                DiagonalBorder = new DiagonalBorder()
            });
            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
            #endregion

            #region Cell Style Format
            var cellStyleFormats = new CellStyleFormats();
            cellStyleFormats.Append(new CellFormat  // Cell style format index 0: no format
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0
            });
            cellStyleFormats.Count = UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);
            #endregion

            #region Cell format
            var cellFormats = new CellFormats();
            cellFormats.Append(new CellFormat());    // Cell format index 0
            cellFormats.Append(new CellFormat   // CellFormat index 1
            {
                NumberFormatId = 14,        // 14 = 'mm-dd-yy'. Standard Date format;
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell format index 2: Standard Number format with 2 decimal placing
            {
                NumberFormatId = 4,        // 4 = '#,##0.00';
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell formt index 3
            {
                NumberFormatId = DATETIME_FORMAT,        // 164 = 'dd/mm/yyyy hh:mm:ss'. Standard Datetime format;
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell format index 4
            {
                NumberFormatId = 3, // 3   #,##0
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat    // Cell format index 5
            {
                NumberFormatId = 4, // 4   #,##0.00
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell format index 6
            {
                NumberFormatId = 10,    // 10  0.00 %,
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell format index 7
            {
                NumberFormatId = DIGITS4_FORMAT,    // Format cellas 4 digits. If less than 4 digits, prepend 0 in front
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            });
            cellFormats.Append(new CellFormat   // Cell format index 8: Cell header
            {
                NumberFormatId = 49,
                FontId = 1,
                FillId = 3,
                BorderId = 1,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true),
                Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center }
            });
            cellFormats.Append(new CellFormat   // Cell format index 9
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 4,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true),
            });
            cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);
            #endregion

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);

            #region Cell styles
            var css = new CellStyles();
            css.Append(new CellStyle
            {
                Name = StringValue.FromString("Normal"),
                FormatId = 0,
                BuiltinId = 0
            });
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            stylesheet.Append(css);
            #endregion

            var dfs = new DifferentialFormats { Count = 0 };
            stylesheet.Append(dfs);
            var tss = new TableStyles
            {
                Count = 0,
                DefaultTableStyle = StringValue.FromString("TableStyleMedium9"),
                DefaultPivotStyle = StringValue.FromString("PivotStyleLight16")
            };
            stylesheet.Append(tss);

            return stylesheet;
        }
    }
}
