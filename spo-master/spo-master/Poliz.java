import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Stack;

public class Poliz {
    private ArrayList<Token> result = new ArrayList<>();

    private boolean higherPriority(String op1, String op2) {
        return (op1.equals("*") || op1.equals("/")) && (op2.equals("+") || op2.equals("-"));
    }

    private LinkedList<Token> getExprInBrackets(LinkedList<Token> tokens, String type) {
        LinkedList<Token> body = new LinkedList<>();
        int open = 1;
        int close = 0;
        while (open != close) {
            Token token = tokens.poll();
            if (token.getType().equals("OPEN_" + type)) {
                open++;
            } else if (token.getType().equals("CLOSE_" + type)) {
                close++;
            }
            body.add(token);
        }
        body.removeLast();
        return body;
    }

    private void whileToPoliz(LinkedList<Token> tokens) {
        boolean stop = false;
        int start = result.size();
        int end = 0;
        Token labelStart;
        Token labelEnd = null;

        while (!stop) {
            Token token = tokens.poll();
            String type = token.getType();
            switch (type) {
                case "OPEN_BR":
                    LinkedList<Token> condition = getExprInBrackets(tokens, "BRACKET");
                    toPoliz(condition);
                    tokens.addFirst(new Token(")", "CLOSE_BR"));
                    break;
                case "CLOSE_BR":
                    labelEnd = new Token("", "LABEL_END");
                    result.add(labelEnd);
                    result.add(new Token("!F", "GOTO_BY_FALSE"));
                    break;
                case "OP_BR":
                    LinkedList<Token> body = getExprInBrackets(tokens, "BRACE");
                    toPoliz(body);
                    tokens.addFirst(new Token("}", "CL_BR"));
                    break;
                case "CL_BR":
                    labelStart = new Token(String.valueOf(start), "LABEL_START");
                    result.add(labelStart);
                    result.add(new Token("!", "GOTO"));
                    end = result.size();
                    stop = true;
                    break;
            }
        }
        labelEnd.setText(String.valueOf(end));
    }



    private void ifToPoliz(LinkedList<Token> tokens) {
        boolean stop = false;
        int end = 0;
        Token labelEnd = null;

        while (!stop) {
            Token token = tokens.poll();
            String type = token.getType();
            switch (type) {
                case "OPEN_BR":
                    LinkedList<Token> condition = getExprInBrackets(tokens, "BRACKET");
                    toPoliz(condition);
                    tokens.addFirst(new Token(")", "CLOSE_BR"));
                    break;
                case "CLOSE_BR":
                    labelEnd = new Token("", "LABEL_END");
                    result.add(labelEnd);
                    result.add(new Token("!F", "GOTO_BY_FALSE"));
                    break;
                case "OP_BR":
                    LinkedList<Token> body = getExprInBrackets(tokens, "BRACE");
                    toPoliz(body);
                    tokens.addFirst(new Token("}", "CL_BR"));
                    break;
                case "CL_BR":
                    end = result.size();
                    stop = true;
                    break;
            }
        }
        labelEnd.setText(String.valueOf(end));
    }

    public ArrayList<Token> toPoliz(LinkedList<Token> tokens) {
        Stack<Token> stack = new Stack<>();
        Token upperInStack;

        while (!tokens.isEmpty()) {
            Token token = tokens.poll();
            String type = token.getType();
            switch (type) {
                //Если следующий токен - операнд, то он сразу добавляется в ПОЛИЗ
                case "VAR":
                case "CONST_INT":
                case "CONST_FLOAT":
                case "STRUCT_NAME":
                    result.add(token);
                    break;
                case "ARITHMETIC_OP":
                    //Вытолкнуть из стека все операции с более высоким приоритетом
                    while (!stack.isEmpty() && (upperInStack = stack.peek()).getType().equals("ARITHMETIC_OP") &&
                            higherPriority(upperInStack.getText(), token.getText())) {
                        result.add(stack.pop());
                    }
                    stack.push(token);
                    break;
                case "ASSIGN_OP":
                case "OPEN_BR":
                case "OPERATION_KW":
                case "PRINT_KW":
                case "IS_KW":
                case "COMP_OP":
                case "GET":
                    stack.push(token);
                    break;
                case "CLOSE_BR":
                    //Выталкивать из стека все операции пока не встретится открывающаяся скобка
                    while (!stack.isEmpty() && !(upperInStack = stack.pop()).getType().equals("OPEN_BR")) {
                        result.add(upperInStack);
                    }
                    break;
                case "EXPR_END":
                    while (!stack.isEmpty()) {
                        result.add(stack.pop());
                    }
                    break;
                case "WHILE_KW":
                    whileToPoliz(tokens);
                    break;

                case "IF_KW":
                    ifToPoliz(tokens);
                    break;
            }
        }

        //Выталкиваем оставшееся содержимое стека в ПОЛИЗ
        while (!stack.isEmpty()) {
            result.add(stack.pop());
        }

        return result;
    }
}
