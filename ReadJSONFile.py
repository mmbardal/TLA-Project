import json
import OriginalData
import TemporaryData

global jsonFileData

def WriteToJSONFile():
    finalAnswer = {}
    finalAnswer["states"] = str(ReturnStatesNames())
    finalAnswer["input_symbols"]=str(ReturnInputsForJsonFile())
    finalAnswer["transitions"]=ReturnTransitions()
    finalAnswer["initial_state"]="q0"
    i=[]
    i.append("q0")
    finalAnswer["final_states"]=ReturnFinalStates()
    print(finalAnswer["final_states"])
    json_object=json.dumps(finalAnswer,indent=4)
    with open("output.json","w") as outfile:
        outfile.write(json_object)

def ReturnFinalStates():
    return "{"+",".join(TemporaryData.finalStates)+"}"          

def ReturnInputsForJsonFile():
    i=[]
    for item in OriginalData.input_symbols:
        i.append('\''+item+'\'')
    i="{"+",".join(i)+"}"
    return i

def ReturnTransitions():
    finalAnswer={}
    for item in TemporaryData.completedStates:
        name="".join(item["states"])
        tran={}
        for alphabet in item["Transition"]:
            i=[]
            i.append(''.join(item["Transition"][alphabet]))
            tran[alphabet]=str(i[0])
        finalAnswer[name]=tran
    return finalAnswer

def IsItFinalState(state):
    for finalState in OriginalData.final_states:
        for item in state:
            if finalState==item:
                return True
    return False

def ReturnStatesNames():
    names = []
    for item in TemporaryData.completedStates:
        n='\''+"".join(item["states"])+'\''
        if(IsItFinalState(item["states"])):
            TemporaryData.finalStates.append(n)
        names.append(n)
   # names = set(names)
    a="{"+",".join(names)+"}"
    return a


#initialize original data
def init():
    ReadJSONFile()
    ReturnFinalStatesList()
    ReturnInitialStateString()
    ReturnTransitionsDictionary()
    ReturnFinalAndNonfinalStatesList()
    ReturnInputSymbolsList()

#open and read contents of input.json 
def ReadJSONFile():

    jsonFilePath = "input2.json"
    jsonFile = open(jsonFilePath)
    global jsonFileData
    jsonFileData = json.load(jsonFile)

#seperate final states
def ReturnFinalStatesList():
    answer = []
    for finalState in eval(jsonFileData["final_states"]):
        answer.append(finalState)
    OriginalData.final_states = answer

#seperate initial state
def ReturnInitialStateString():
    OriginalData.initial_state = jsonFileData["initial_state"]

# an example of return answer :
# {
#   q0 :
#   {
#       "a" : { "q1" , "q2 "}
#    }
# }

#seperate transitions
def ReturnTransitionsDictionary():
    transitions = jsonFileData["transitions"]

    answer = {}

    for state in transitions.keys():
        t = {}
        for alphabet in transitions[state].keys():
          #      t[alphabet] = eval(transitions[state][alphabet])
            t[alphabet] = []
            for i in eval(transitions[state][alphabet]):
                t[alphabet].append(i)
        answer[state] = t

    OriginalData.originalTransitions = answer

#seperate all states
def ReturnFinalAndNonfinalStatesList():
    OriginalData.originalStates = eval(jsonFileData["states"])

#seperate input symbols
def ReturnInputSymbolsList():
    OriginalData.input_symbols = eval(jsonFileData["input_symbols"])
