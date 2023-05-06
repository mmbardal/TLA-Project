import ReadJSONFile
import OriginalData
import TemporaryData
import changeNFAStatetoDFAState

ReadJSONFile.init()


def ReturnTransitionsForOneInputSymbolAndOneInputState(
        inputState_string,
        inputSymbol_string
):
    answer = []
    if(inputState_string in OriginalData.originalTransitions):
        if(inputSymbol_string in OriginalData.originalTransitions[inputState_string]):
            for item in set(OriginalData.originalTransitions[inputState_string][inputSymbol_string]):
                answer.append(item)
                for i in changeNFAStatetoDFAState.FindLambdaTransitions(item):
                    answer.append(i)
    return answer

def ReturnReturnTransitionsForOneInputSymbolAndMultipleState(
        inputStates_List,
        inputSymbol_String
):
    answer = []
    for state in inputStates_List:
        list1=ReturnTransitionsForOneInputSymbolAndOneInputState(state,inputSymbol_String)
        for item in list1:
            answer.append(item)
    #to make states unique
    answer= (list(set(answer)))
    TemporaryData.EnqueueState(answer)
    return answer

def ReturnTransitionsForMultipleInputSymbolAndMultipleStates(
    inputStates
    ):

    answer={}
    for alphabet in OriginalData.input_symbols:
        answer[alphabet]=ReturnReturnTransitionsForOneInputSymbolAndMultipleState(inputStates,alphabet)
        if(len(answer[alphabet])==0):
            answer[alphabet].append(OriginalData.Trap)
    return answer

