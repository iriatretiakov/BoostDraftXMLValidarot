# Description 
XMLValidation is consist with two main parts, Parser and Validator. The Parser reads string input and create flat appearance for nodes(Elements) xml. The Validator is process array of Elements and make decision valid xml or not. 

## Parser
Is use a Finite State Machine approach to populate array of Elements with the corresponding type of node (Opening, Closing).
FSM scheme:
![FSM](/sources/scheme.png)

## Validator
The Validator receive a list of Elements and use stack to decided if the input is valid. Element is store in the stack if it type is Opening. If matching element with type Closing is received, that element is removed from th stack, this approach guarantees nested order of xml nodes.