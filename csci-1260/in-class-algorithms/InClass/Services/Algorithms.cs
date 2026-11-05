namespace InClass.Services
{
    public class Algorithms
    {
        // Iterative search method
        public int IterativeSearch(int target, List<int> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == target)
                {
                    return i; // Return the index of the found item
                }
            }
            return -1; // Return -1 if the item is not found
        }

        // Binary search method
        public int BinarySearch(int target, List<int> values)
        {
            values.Sort(); // Ensure the list is sorted before binary search
            int left = 0, right = values.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (values[mid] == target)
                {
                    return mid; // Return the index of the found item
                }
                else if (values[mid] < target)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }
            return -1; // Return -1 if the item is not found
        }

        // Bubble sort method
        public void BubbleSort(List<int> values)
        {
            int n = values.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (values[j] > values[j + 1])
                    {
                        // Swap values[j] and values[j + 1]
                        int temp = values[j];
                        values[j] = values[j + 1];
                        values[j + 1] = temp;
                    }
                }
            }
        }
    }
}
