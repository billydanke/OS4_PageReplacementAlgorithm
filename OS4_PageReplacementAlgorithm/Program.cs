// Operating Systems Homework 4 - Page Replacement Algorithm
// Aaron Barrett


using System;
using System.Collections.Generic;
using System.Linq;

namespace OS4_PageReplacementAlgorithm
{
    interface IPageReplacementAlgorithm
    {
        void UpdatePageReference(List<int> frames, int page);
        int GetPageToRemove(List<int> frames, int page);
    }

    class LRUPageReplacement : IPageReplacementAlgorithm
    {
        // This removes the page from its position in the list and ensures that it is at the end (meaning most recently used)
        public void UpdatePageReference(List<int> frames, int page)
        {
            frames.Remove(page);
            frames.Add(page);
        }

        // This returns the first page in the list (aka the least recently used)
        public int GetPageToRemove(List<int> frames, int page)
        {
            return frames[0];
        }
    }

    class OptimalPageReplacement : IPageReplacementAlgorithm
    {
        private List<int> referenceString;

        public OptimalPageReplacement(List<int> referenceString)
        {
            this.referenceString = referenceString;
        }

        public void UpdatePageReference(List<int> frames, int page)
        {
            // This is just here to make the interface happy, we don't need to update references in this one.
        }

        // This checks for future uses of each frame and finds the frame that will not be used for the longest time.
        // Also, if a frame is never going to be used again it gets picked right away.
        public int GetPageToRemove(List<int> frames, int page)
        {
            int farthestIndex = -1;
            int pageToRemove = frames[0];

            for(int i = 0; i < frames.Count; i++)
            {
                int j = referenceString.IndexOf(frames[i], referenceString.IndexOf(page) + 1);
                if (j == -1) // The frame is never used again
                {
                    return frames[i];
                }
                if(j > farthestIndex) // The frame is used again, and if it is used later than any other it is now up for removal
                {
                    farthestIndex = j;
                    pageToRemove = frames[i];
                }
            }

            // the frame that is up for removal at the end of the loop gets selected
            return pageToRemove;
        }
    }

    class FIFOPageReplacement : IPageReplacementAlgorithm
    {
        private Queue<int> queue = new Queue<int>();

        // I had to include this to make sure the queue gets populated before trying to dequeue.
        // This adds a new page to the queue when it gets loaded.
        public void EnqueuePage(int page)
        {
            queue.Enqueue(page);
        }

        public void UpdatePageReference(List<int> frames, int page)
        {
            // Just like in Optimal, this one is also just here to make the interface happy.
        }

        // Removes the first (aka oldest) page added to the queue.
        public int GetPageToRemove(List<int> frames, int page)
        {
            int pageToRemove = queue.Dequeue();
            queue.Enqueue(page);
            return pageToRemove;
        }
    }

    internal class Program
    {
        private static void SimulatePageReplacement(List<int> referenceString, int frameCount, IPageReplacementAlgorithm algorithm)
        {
            HashSet<int> pageTable = new HashSet<int>();
            List<int> frames = new List<int>();
            int faults = 0;

            for(int i = 0; i < referenceString.Count; i++)
            {
                int page = referenceString[i];
                if(!pageTable.Contains(page)) // The page table does not currently have the page
                {
                    if(pageTable.Count >= frameCount)
                    {
                        int pageToRemove = algorithm.GetPageToRemove(frames, page);
                        pageTable.Remove(pageToRemove);
                        frames.Remove(pageToRemove);
                    }

                    pageTable.Add(page);
                    frames.Add(page);
                    faults++;
                    if(algorithm is FIFOPageReplacement fifoAlgorithm)
                    {
                        fifoAlgorithm.EnqueuePage(page);
                    }
                    Console.WriteLine($"Step {i + 1}: Page fault ({page}) - Page Table: {{{string.Join(", ", pageTable)}}}, Frames: [{string.Join(", ", frames)}], Faults: {faults}");
                }
                else // The page table already contains the page
                {
                    algorithm.UpdatePageReference(frames, page);
                }
            }

            Console.WriteLine($"\nTotal Page Faults: {faults}");
        }

        static void Main(string[] args)
        {
            List<int> referenceString = new List<int> { 1, 2, 3, 4, 1, 2, 5, 1, 2, 3, 4, 5 };
            int frameCount = 4;
            
            // See the README for my efficiency assessment of each algorithm.
            Console.WriteLine("Running LRU Algorithm:");
            SimulatePageReplacement(referenceString, frameCount, new LRUPageReplacement());

            Console.WriteLine("\n\nRunning Optimal Algorithm:");
            SimulatePageReplacement(referenceString,frameCount,new OptimalPageReplacement(referenceString));

            Console.WriteLine("\n\nRunning FIFO Algorithm:");
            SimulatePageReplacement(referenceString,frameCount,new FIFOPageReplacement());
        }
    }
}
