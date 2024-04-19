# Operating Systems Homework 4

## Program Functionality
This program takes in a page reference string and frame count. From there, it will run the reference string
through different page replacement algorithms, namely LRU, Optimal, and FIFO. It shows each step containing faults,
and will give a total fault count for each algorithm.

## Algorithm Assessment
I'll start by saying that each of these have their own strengths and weaknesses, which is definitely something
to take into account.
### Least Recently Used (LRU) Algorithm
This is simple and has a relatively low time complexity. UpdatePageReference removes the page, which can potentially be up to O(n) complexity.
Adding the page to the frame is just O(1), and the GetPageToRemove function is always O(1). The real downside to using LRU is that it
requires frequent list updates, which means higher memory accesses every time a page is referenced.
### Optimal Algorithm
This is definitely the most complex algorithm, and has the highest time complexity. I'm not entirely sure how to calculate the time complexity
of its GetPageToRemove function, but I'll say O(F*M), since F is the frame count and M is the number of references. Either way, more time
needed than the other algorithms. It also has to store the entire reference string and accesses it often, which means higher memory usage.
It has the lowest fault count, but to do that it needs to know future requests, so I'm not sure how practical it is in reality.
### First In First Out (FIFO) Algorithm
This is pretty simple and has the lowest time complexity, with just an O(1) complexity for queue operations. Using a queue also means
that it is very memory access efficient. On the downside, it has the highest number of faults, and it can very easily remove pages that
are frequently used just because they reached the end of the queue.

## Execution Instructions
Make sure you have .NET 8 installed. It can be installed from the Visual Studio installer. From there, just run it through the visual studio
solution file .sln, just like you would a C++ project.