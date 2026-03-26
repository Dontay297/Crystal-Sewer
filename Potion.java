import java.util.*;

class Potion extends BeyonderItem {
    private String name;
    private String description;

    Potion(String name, String description) {
        super(name, description);
    }

  public String getName(){
    return name;
  }

  public String getDescription(){
    return description;
  }


}