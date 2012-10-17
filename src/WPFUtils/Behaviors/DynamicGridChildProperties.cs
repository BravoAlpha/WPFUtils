using System;
using System.Windows;

namespace WPFUtils.Behaviors
{
    public class DynamicGridChildProperties : Freezable
    {
        private static readonly DynamicGridChildProperties DefaultChildProperties = 
            new DynamicGridChildProperties { Column = 0, Row = 0, RowSpan = 1, ColumnSpan = 1, Visibility = Visibility.Collapsed };

        private int m_index;
        private int m_row;
        private int m_column;
        private int m_rowSpan;
        private int m_columnSpan;

        public DynamicGridChildProperties()
        {
            RowSpan = 1;
            ColumnSpan = 1;
            Visibility = Visibility.Visible;
        }

        public int Index
        {
            get { return m_index; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Index must be a positive number");

                m_index = value;
            }
        }

        public int Row
        {
            get { return m_row; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Row must be a positive number");

                m_row = value;
            }
        }

        public int Column
        {
            get { return m_column; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Column must be a positive number");

                m_column = value;
            }
        }

        public int RowSpan
        {
            get { return m_rowSpan; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "RowSpan must be at least 1");

                m_rowSpan = value;
            }
        }

        public int ColumnSpan
        {
            get { return m_columnSpan; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "ColumnSpan must be at least 1");

                m_columnSpan = value;
            }
        }

        public Visibility Visibility { get; set; }

        public static DynamicGridChildProperties Default
        {
            get { return DefaultChildProperties; }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DynamicGridChildProperties();
        }
    }

    public class DynamicGridChildPropertiesCollection : FreezableCollection<DynamicGridChildProperties>
    {

    }
}