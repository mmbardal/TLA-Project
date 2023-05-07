from ReadJSONFile import init
import OriginalData

init()

def CheckString(stri,num,state):
    if(num==len(stri)-1 and state in OriginalData.final_states):
        return "Accepted"
    elif(num==len(stri)-1 and state not in OriginalData.final_states):
        return "Rejected"
    
    currentAlphabet=stri[num]
    if(currentAlphabet not in OriginalData.originalTransitions[state]):
        return "Rejected"
    else:
        for sattes in OriginalData.originalTransitions[state][currentAlphabet]:
            ans=CheckString(stri,num+1,sattes)
            if(ans=="Accepted"):
                return "Accepted"
            
        return "Rejected"
    
print(CheckString("abaa",-1,"A"))