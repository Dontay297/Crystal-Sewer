import java.util.*;

class AdvancementRitual extends BeyonderItem {
    private String Name;
    private String Description;
    private String requirements; 
    private int diffculty; 

    AdvancementRitual(String Name, String Description, String requirements, int diffculty) {
        super(Name, Description);
        this.requirements = requirements;
        this.diffculty = diffculty;
    }

   
    public String getRequirements(){
        return requirements;
    }

    public int getDiffculty(){
        return diffculty;
    }

    @Override
    public String toString(){
      return super.toString() + "\nrequirement: "+ requirements +
      "\nDiffculty: " + diffculty;

    }
        
    

}