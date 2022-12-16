// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
namespace NSKurtsTools;

public partial class KurtsTools{
    public record struct IntSpan{
        private int _min;
        private int _max;

        public IntSpan(int exactNumber){
            _min = exactNumber;
            _max = exactNumber;
        }

        public IntSpan(int min, int max){
            if (min > max) throw new ArgumentException("Minimum value is greater than maximum value");
            _min = min;
            _max = max;
        }

        public int Min{
            get => _min;
            set{
                if (value > _max) throw new ArgumentException("Minimum value is greater than maximum value");
                _min = value;
            }
        }

        public int Max{
            get => _max;
            set{
                if (value < _min) throw new ArgumentException("Maximum value is less than Minimum value");
                _max = value;
            }
        }
    }
}