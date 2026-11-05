using InClass.Services;

namespace InClass.Services
{

    public interface iAlgorithms

    {

        public int IterativeSearch(int toFind, List<int> toSearch);



        public int BinarySearch(int toFind, List<int> toSearch);



        public void BubbleSort(List<int> toSort);

    }

}
public class Algorithms : iAlgorithms

{

    public int IterativeSearch(int toFind, List<int> toSearch)

    {

        for (int i = 0; i < toSearch.Count; i++)

        {

            if (toSearch[i] == toFind) return i;

        }



        return -1;

    }



    public int BinarySearch(int toFind, List<int> toSearch)

    {

        //Base Cases

        if (toSearch.Count == 0) return -1;

        if (toSearch.Count == 1)

        {

            if (toSearch[0] == toFind) return 0;

            return -1;

        }



        int mid = toSearch.Count / 2;



        if (toSearch[mid] == toFind) return mid;



        //left side

        if (toSearch[mid] > toFind)

        {

            List<int> lower = toSearch.GetRange(0, mid);

            return BinarySearch(toFind, lower);

        }



        List<int> higher = toSearch.GetRange(mid + 1, toSearch.Count - (mid + 1));

        int result = BinarySearch(toFind, higher);

        if (result == -1) return -1;

        else return mid + 1 + result;

    }



    public void BubbleSort(List<int> toSort)

    {

        for (int i = 0; i < toSort.Count; i++)

        {

            for (int j = 0; j < toSort.Count - i - 1; j++)

            {

                if (toSort[j] > toSort[j + 1])

                {

                    int temp = toSort[j];

                    toSort[j] = toSort[j + 1];

                    toSort[j + 1] = temp;

                }

            }

        }

    }

}



