using System;
using System.Linq;
using System.Windows;

namespace WPFUtils.Behaviors
{
    public class DynamicGridFormation : Freezable
    {
        private int m_numberOfChildren;
        private int m_numberOfRows;
        private int m_numberOfColumns;

        public DynamicGridFormation()
        {
            ChildProperties = new DynamicGridChildPropertiesCollection();
        }

        public int NumberOfChildren
        {
            get { return m_numberOfChildren; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "NumberOfChildren must be a positive number");

                m_numberOfChildren = value;
            }
        }

        public int NumberOfRows
        {
            get { return m_numberOfRows; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "NumberOfRows must be at least 1");

                m_numberOfRows = value;
            }
        }

        public int NumberOfColumns
        {
            get { return m_numberOfColumns; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "NumberOfColumns must be at least 1");

                m_numberOfColumns = value;
            }
        }

        public DynamicGridChildPropertiesCollection ChildProperties { get; set; }

        public DynamicGridChildProperties GetChildPropertiesByIndex(int index)
        {
            DynamicGridChildProperties matchingChild = ChildProperties.FirstOrDefault(child => child.Index == index);
            return matchingChild ?? DynamicGridChildProperties.Default;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DynamicGridFormation();
        }
    }

    public class DynamicGridFormationCollection : FreezableCollection<DynamicGridFormation>
    {

    }
}