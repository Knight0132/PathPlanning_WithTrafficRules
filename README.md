# Path Planning With Traffic Rules
The repository is for the Bachelor's degree thesis. It is about indoor map data structure considering some traffic rules. This is a project which is the simulation of the pathplanning with traffic rules in the warehouse for robots. 

## Settings
Platform: **Unity3D**

The compiler version is **2021.3.32f1c1**.

## Algorithms
In this project, we use the tradictional A* algorithm, and the improved A* algorithm to test our indoor map data structure. 
For the tradictional A* algorithm, we use distance as the cost to calculate the function g(x). And the h(x) function is the Euclid distance function.
But in the improved A* algorithm, we use the time as the cost to calculate the functions. We set the speed as the input parameter.
When selecting the neighbors, the algorithm will detect whether the neighbor is intersection, if it is, the speed will multiply a deacceleration factor. 
And we do this only for compatibility with the newly proposed indoor map data structure which contains the traffic rules information.

## Indoor Map Data Structure
The indoor map data structure contains some information about the indoor traffic rules, which will be useful for the robots. And the json schema can be seem here:
[indoorjson3](https://github.com/Knight0132/indoorjson3-python).
