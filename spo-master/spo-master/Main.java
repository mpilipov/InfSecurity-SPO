import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Queue;

public class Main {
    public static void main(String[] args) throws Exception {
        String example = "let a is set; a add 1; a add 1; a add 1; print a; a add 2; print a; a has 1; a has 2; a has 3;" +
                "a remove 1; print a; a has 2; a has 1;";
        Queue<Token> tokens = Lexer.getTokenList(example);
        Parser parser = new Parser();
        if (!parser.parse(tokens)) {
            return;
        }
        Poliz poliz = new Poliz();
        ArrayList<Token> p = poliz.toPoliz((LinkedList<Token>) tokens);
        StackMachine sm = new StackMachine(parser.getTable(), p);
        sm.calculate();
    }
}
