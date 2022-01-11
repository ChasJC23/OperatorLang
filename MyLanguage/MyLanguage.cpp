// MyLanguage.cpp : Defines the entry point for the application.
//

#include "MyLanguage.h"

using namespace std;

enum Token {
	EoF,
	IntLiteral,
	// Identifier,
	Add,
	Subtract,
	Multiply,
	Divide,
	OpenParens,
	CloseParens,
};

class Tokeniser {
public:

	Tokeniser(istream *stream) {
		this->stream = stream;
	}
	Token getCurrentToken() {
		return this->currentToken;
	}

private:

	const istream *stream;
	char currentChar;
	Token currentToken;

	void nextChar() {
		char ch;
		*stream >> ch;
	}
};

int main()
{
	cout << "Hello CMake." << endl;
	return 0;
}
