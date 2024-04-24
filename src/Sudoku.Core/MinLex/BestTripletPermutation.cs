namespace Sudoku.MinLex;

internal struct BestTripletPermutation
{
	public static readonly int[,] Perm = { { 0, 1, 2 }, { 0, 2, 1 }, { 1, 0, 2 }, { 1, 2, 0 }, { 2, 0, 1 }, { 2, 1, 0 } };

	public static readonly BestTripletPermutation[,] BestTripletPermutations =
	{
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[0][0]=[  0][     0]->{111,     0,0}
			new() { bestResult = 0, resultMask = 1, resultNumBits = 1 },	//[0][1]=[  0][     1]->{  0,     1,1}
			new() { bestResult = 0, resultMask = 2, resultNumBits = 1 },	//[0][2]=[  0][    10]->{  0,    10,1}
			new() { bestResult = 0, resultMask = 3, resultNumBits = 2 },	//[0][3]=[  0][    11]->{  0,    11,2}
			new() { bestResult = 0, resultMask = 4, resultNumBits = 1 },	//[0][4]=[  0][   100]->{  0,   100,1}
			new() { bestResult = 0, resultMask = 5, resultNumBits = 2 },	//[0][5]=[  0][   101]->{  0,   101,2}
			new() { bestResult = 0, resultMask = 6, resultNumBits = 2 },	//[0][6]=[  0][   110]->{  0,   110,2}
			new() { bestResult = 0, resultMask = 7, resultNumBits = 3 },	//[0][7]=[  0][   111]->{  0,   111,3}
			new() { bestResult = 0, resultMask = 8, resultNumBits = 1 },	//[0][8]=[  0][  1000]->{  0,  1000,1}
			new() { bestResult = 0, resultMask = 9, resultNumBits = 2 },	//[0][9]=[  0][  1001]->{  0,  1001,2}
			new() { bestResult = 0, resultMask = 10, resultNumBits = 2 },	//[0][10]=[  0][  1010]->{  0,  1010,2}
			new() { bestResult = 0, resultMask = 11, resultNumBits = 3 },	//[0][11]=[  0][  1011]->{  0,  1011,3}
			new() { bestResult = 0, resultMask = 12, resultNumBits = 2 },	//[0][12]=[  0][  1100]->{  0,  1100,2}
			new() { bestResult = 0, resultMask = 13, resultNumBits = 3 },	//[0][13]=[  0][  1101]->{  0,  1101,3}
			new() { bestResult = 0, resultMask = 14, resultNumBits = 3 },	//[0][14]=[  0][  1110]->{  0,  1110,3}
			new() { bestResult = 0, resultMask = 15, resultNumBits = 4 },	//[0][15]=[  0][  1111]->{  0,  1111,4}
			new() { bestResult = 0, resultMask = 16, resultNumBits = 1 },	//[0][16]=[  0][ 10000]->{  0, 10000,1}
			new() { bestResult = 0, resultMask = 17, resultNumBits = 2 },	//[0][17]=[  0][ 10001]->{  0, 10001,2}
			new() { bestResult = 0, resultMask = 18, resultNumBits = 2 },	//[0][18]=[  0][ 10010]->{  0, 10010,2}
			new() { bestResult = 0, resultMask = 19, resultNumBits = 3 },	//[0][19]=[  0][ 10011]->{  0, 10011,3}
			new() { bestResult = 0, resultMask = 20, resultNumBits = 2 },	//[0][20]=[  0][ 10100]->{  0, 10100,2}
			new() { bestResult = 0, resultMask = 21, resultNumBits = 3 },	//[0][21]=[  0][ 10101]->{  0, 10101,3}
			new() { bestResult = 0, resultMask = 22, resultNumBits = 3 },	//[0][22]=[  0][ 10110]->{  0, 10110,3}
			new() { bestResult = 0, resultMask = 23, resultNumBits = 4 },	//[0][23]=[  0][ 10111]->{  0, 10111,4}
			new() { bestResult = 0, resultMask = 24, resultNumBits = 2 },	//[0][24]=[  0][ 11000]->{  0, 11000,2}
			new() { bestResult = 0, resultMask = 25, resultNumBits = 3 },	//[0][25]=[  0][ 11001]->{  0, 11001,3}
			new() { bestResult = 0, resultMask = 26, resultNumBits = 3 },	//[0][26]=[  0][ 11010]->{  0, 11010,3}
			new() { bestResult = 0, resultMask = 27, resultNumBits = 4 },	//[0][27]=[  0][ 11011]->{  0, 11011,4}
			new() { bestResult = 0, resultMask = 28, resultNumBits = 3 },	//[0][28]=[  0][ 11100]->{  0, 11100,3}
			new() { bestResult = 0, resultMask = 29, resultNumBits = 4 },	//[0][29]=[  0][ 11101]->{  0, 11101,4}
			new() { bestResult = 0, resultMask = 30, resultNumBits = 4 },	//[0][30]=[  0][ 11110]->{  0, 11110,4}
			new() { bestResult = 0, resultMask = 31, resultNumBits = 5 },	//[0][31]=[  0][ 11111]->{  0, 11111,5}
			new() { bestResult = 0, resultMask = 32, resultNumBits = 1 },	//[0][32]=[  0][100000]->{  0,100000,1}
			new() { bestResult = 0, resultMask = 33, resultNumBits = 2 },	//[0][33]=[  0][100001]->{  0,100001,2}
			new() { bestResult = 0, resultMask = 34, resultNumBits = 2 },	//[0][34]=[  0][100010]->{  0,100010,2}
			new() { bestResult = 0, resultMask = 35, resultNumBits = 3 },	//[0][35]=[  0][100011]->{  0,100011,3}
			new() { bestResult = 0, resultMask = 36, resultNumBits = 2 },	//[0][36]=[  0][100100]->{  0,100100,2}
			new() { bestResult = 0, resultMask = 37, resultNumBits = 3 },	//[0][37]=[  0][100101]->{  0,100101,3}
			new() { bestResult = 0, resultMask = 38, resultNumBits = 3 },	//[0][38]=[  0][100110]->{  0,100110,3}
			new() { bestResult = 0, resultMask = 39, resultNumBits = 4 },	//[0][39]=[  0][100111]->{  0,100111,4}
			new() { bestResult = 0, resultMask = 40, resultNumBits = 2 },	//[0][40]=[  0][101000]->{  0,101000,2}
			new() { bestResult = 0, resultMask = 41, resultNumBits = 3 },	//[0][41]=[  0][101001]->{  0,101001,3}
			new() { bestResult = 0, resultMask = 42, resultNumBits = 3 },	//[0][42]=[  0][101010]->{  0,101010,3}
			new() { bestResult = 0, resultMask = 43, resultNumBits = 4 },	//[0][43]=[  0][101011]->{  0,101011,4}
			new() { bestResult = 0, resultMask = 44, resultNumBits = 3 },	//[0][44]=[  0][101100]->{  0,101100,3}
			new() { bestResult = 0, resultMask = 45, resultNumBits = 4 },	//[0][45]=[  0][101101]->{  0,101101,4}
			new() { bestResult = 0, resultMask = 46, resultNumBits = 4 },	//[0][46]=[  0][101110]->{  0,101110,4}
			new() { bestResult = 0, resultMask = 47, resultNumBits = 5 },	//[0][47]=[  0][101111]->{  0,101111,5}
			new() { bestResult = 0, resultMask = 48, resultNumBits = 2 },	//[0][48]=[  0][110000]->{  0,110000,2}
			new() { bestResult = 0, resultMask = 49, resultNumBits = 3 },	//[0][49]=[  0][110001]->{  0,110001,3}
			new() { bestResult = 0, resultMask = 50, resultNumBits = 3 },	//[0][50]=[  0][110010]->{  0,110010,3}
			new() { bestResult = 0, resultMask = 51, resultNumBits = 4 },	//[0][51]=[  0][110011]->{  0,110011,4}
			new() { bestResult = 0, resultMask = 52, resultNumBits = 3 },	//[0][52]=[  0][110100]->{  0,110100,3}
			new() { bestResult = 0, resultMask = 53, resultNumBits = 4 },	//[0][53]=[  0][110101]->{  0,110101,4}
			new() { bestResult = 0, resultMask = 54, resultNumBits = 4 },	//[0][54]=[  0][110110]->{  0,110110,4}
			new() { bestResult = 0, resultMask = 55, resultNumBits = 5 },	//[0][55]=[  0][110111]->{  0,110111,5}
			new() { bestResult = 0, resultMask = 56, resultNumBits = 3 },	//[0][56]=[  0][111000]->{  0,111000,3}
			new() { bestResult = 0, resultMask = 57, resultNumBits = 4 },	//[0][57]=[  0][111001]->{  0,111001,4}
			new() { bestResult = 0, resultMask = 58, resultNumBits = 4 },	//[0][58]=[  0][111010]->{  0,111010,4}
			new() { bestResult = 0, resultMask = 59, resultNumBits = 5 },	//[0][59]=[  0][111011]->{  0,111011,5}
			new() { bestResult = 0, resultMask = 60, resultNumBits = 4 },	//[0][60]=[  0][111100]->{  0,111100,4}
			new() { bestResult = 0, resultMask = 61, resultNumBits = 5 },	//[0][61]=[  0][111101]->{  0,111101,5}
			new() { bestResult = 0, resultMask = 62, resultNumBits = 5 },	//[0][62]=[  0][111110]->{  0,111110,5}
			new() { bestResult = 0, resultMask = 63, resultNumBits = 6 },	//[0][63]=[  0][111111]->{  0,111111,6}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[1][0]=[  1][     0]->{111,     0,0}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][1]=[  1][     1]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 2, resultNumBits = 1 },	//[1][2]=[  1][    10]->{ 10,    10,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][3]=[  1][    11]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][4]=[  1][   100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][5]=[  1][   101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][6]=[  1][   110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][7]=[  1][   111]->{  1,   101,2}
			new() { bestResult = 4, resultMask = 8, resultNumBits = 1 },	//[1][8]=[  1][  1000]->{100,  1000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][9]=[  1][  1001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 2, resultNumBits = 1 },	//[1][10]=[  1][  1010]->{ 10,    10,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][11]=[  1][  1011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][12]=[  1][  1100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][13]=[  1][  1101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][14]=[  1][  1110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][15]=[  1][  1111]->{  1,   101,2}
			new() { bestResult = 2, resultMask = 16, resultNumBits = 1 },	//[1][16]=[  1][ 10000]->{ 10, 10000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][17]=[  1][ 10001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 18, resultNumBits = 2 },	//[1][18]=[  1][ 10010]->{ 10, 10010,2}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][19]=[  1][ 10011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][20]=[  1][ 10100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][21]=[  1][ 10101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][22]=[  1][ 10110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][23]=[  1][ 10111]->{  1,   101,2}
			new() { bestResult = 2, resultMask = 16, resultNumBits = 1 },	//[1][24]=[  1][ 11000]->{ 10, 10000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][25]=[  1][ 11001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 18, resultNumBits = 2 },	//[1][26]=[  1][ 11010]->{ 10, 10010,2}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][27]=[  1][ 11011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][28]=[  1][ 11100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][29]=[  1][ 11101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][30]=[  1][ 11110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][31]=[  1][ 11111]->{  1,   101,2}
			new() { bestResult = 4, resultMask = 32, resultNumBits = 1 },	//[1][32]=[  1][100000]->{100,100000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][33]=[  1][100001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 2, resultNumBits = 1 },	//[1][34]=[  1][100010]->{ 10,    10,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][35]=[  1][100011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][36]=[  1][100100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][37]=[  1][100101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][38]=[  1][100110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][39]=[  1][100111]->{  1,   101,2}
			new() { bestResult = 4, resultMask = 40, resultNumBits = 2 },	//[1][40]=[  1][101000]->{100,101000,2}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][41]=[  1][101001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 2, resultNumBits = 1 },	//[1][42]=[  1][101010]->{ 10,    10,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][43]=[  1][101011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][44]=[  1][101100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][45]=[  1][101101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][46]=[  1][101110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][47]=[  1][101111]->{  1,   101,2}
			new() { bestResult = 2, resultMask = 16, resultNumBits = 1 },	//[1][48]=[  1][110000]->{ 10, 10000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][49]=[  1][110001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 18, resultNumBits = 2 },	//[1][50]=[  1][110010]->{ 10, 10010,2}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][51]=[  1][110011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][52]=[  1][110100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][53]=[  1][110101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][54]=[  1][110110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][55]=[  1][110111]->{  1,   101,2}
			new() { bestResult = 2, resultMask = 16, resultNumBits = 1 },	//[1][56]=[  1][111000]->{ 10, 10000,1}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][57]=[  1][111001]->{  1,     1,1}
			new() { bestResult = 2, resultMask = 18, resultNumBits = 2 },	//[1][58]=[  1][111010]->{ 10, 10010,2}
			new() { bestResult = 1, resultMask = 1, resultNumBits = 1 },	//[1][59]=[  1][111011]->{  1,     1,1}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][60]=[  1][111100]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][61]=[  1][111101]->{  1,   101,2}
			new() { bestResult = 1, resultMask = 4, resultNumBits = 1 },	//[1][62]=[  1][111110]->{  1,   100,1}
			new() { bestResult = 1, resultMask = 5, resultNumBits = 2 },	//[1][63]=[  1][111111]->{  1,   101,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[2][0]=[ 10][     0]->{111,     0,0}
			new() { bestResult = 2, resultMask = 1, resultNumBits = 1 },	//[2][1]=[ 10][     1]->{ 10,     1,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][2]=[ 10][    10]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][3]=[ 10][    11]->{  1,    10,1}
			new() { bestResult = 4, resultMask = 4, resultNumBits = 1 },	//[2][4]=[ 10][   100]->{100,   100,1}
			new() { bestResult = 2, resultMask = 1, resultNumBits = 1 },	//[2][5]=[ 10][   101]->{ 10,     1,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][6]=[ 10][   110]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][7]=[ 10][   111]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][8]=[ 10][  1000]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][9]=[ 10][  1001]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][10]=[ 10][  1010]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][11]=[ 10][  1011]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][12]=[ 10][  1100]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][13]=[ 10][  1101]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][14]=[ 10][  1110]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][15]=[ 10][  1111]->{  1,  1010,2}
			new() { bestResult = 4, resultMask = 16, resultNumBits = 1 },	//[2][16]=[ 10][ 10000]->{100, 10000,1}
			new() { bestResult = 2, resultMask = 1, resultNumBits = 1 },	//[2][17]=[ 10][ 10001]->{ 10,     1,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][18]=[ 10][ 10010]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][19]=[ 10][ 10011]->{  1,    10,1}
			new() { bestResult = 4, resultMask = 20, resultNumBits = 2 },	//[2][20]=[ 10][ 10100]->{100, 10100,2}
			new() { bestResult = 2, resultMask = 1, resultNumBits = 1 },	//[2][21]=[ 10][ 10101]->{ 10,     1,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][22]=[ 10][ 10110]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][23]=[ 10][ 10111]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][24]=[ 10][ 11000]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][25]=[ 10][ 11001]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][26]=[ 10][ 11010]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][27]=[ 10][ 11011]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][28]=[ 10][ 11100]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][29]=[ 10][ 11101]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][30]=[ 10][ 11110]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][31]=[ 10][ 11111]->{  1,  1010,2}
			new() { bestResult = 2, resultMask = 32, resultNumBits = 1 },	//[2][32]=[ 10][100000]->{ 10,100000,1}
			new() { bestResult = 2, resultMask = 33, resultNumBits = 2 },	//[2][33]=[ 10][100001]->{ 10,100001,2}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][34]=[ 10][100010]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][35]=[ 10][100011]->{  1,    10,1}
			new() { bestResult = 2, resultMask = 32, resultNumBits = 1 },	//[2][36]=[ 10][100100]->{ 10,100000,1}
			new() { bestResult = 2, resultMask = 33, resultNumBits = 2 },	//[2][37]=[ 10][100101]->{ 10,100001,2}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][38]=[ 10][100110]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][39]=[ 10][100111]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][40]=[ 10][101000]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][41]=[ 10][101001]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][42]=[ 10][101010]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][43]=[ 10][101011]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][44]=[ 10][101100]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][45]=[ 10][101101]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][46]=[ 10][101110]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][47]=[ 10][101111]->{  1,  1010,2}
			new() { bestResult = 2, resultMask = 32, resultNumBits = 1 },	//[2][48]=[ 10][110000]->{ 10,100000,1}
			new() { bestResult = 2, resultMask = 33, resultNumBits = 2 },	//[2][49]=[ 10][110001]->{ 10,100001,2}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][50]=[ 10][110010]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][51]=[ 10][110011]->{  1,    10,1}
			new() { bestResult = 2, resultMask = 32, resultNumBits = 1 },	//[2][52]=[ 10][110100]->{ 10,100000,1}
			new() { bestResult = 2, resultMask = 33, resultNumBits = 2 },	//[2][53]=[ 10][110101]->{ 10,100001,2}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][54]=[ 10][110110]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 2, resultNumBits = 1 },	//[2][55]=[ 10][110111]->{  1,    10,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][56]=[ 10][111000]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][57]=[ 10][111001]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][58]=[ 10][111010]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][59]=[ 10][111011]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][60]=[ 10][111100]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 8, resultNumBits = 1 },	//[2][61]=[ 10][111101]->{  1,  1000,1}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][62]=[ 10][111110]->{  1,  1010,2}
			new() { bestResult = 1, resultMask = 10, resultNumBits = 2 },	//[2][63]=[ 10][111111]->{  1,  1010,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[3][0]=[ 11][     0]->{111,     0,0}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][1]=[ 11][     1]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][2]=[ 11][    10]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][3]=[ 11][    11]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 4, resultNumBits = 1 },	//[3][4]=[ 11][   100]->{101,   100,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][5]=[ 11][   101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][6]=[ 11][   110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][7]=[ 11][   111]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 8, resultNumBits = 1 },	//[3][8]=[ 11][  1000]->{101,  1000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][9]=[ 11][  1001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][10]=[ 11][  1010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][11]=[ 11][  1011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 12, resultNumBits = 2 },	//[3][12]=[ 11][  1100]->{101,  1100,2}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][13]=[ 11][  1101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][14]=[ 11][  1110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][15]=[ 11][  1111]->{ 11,    11,2}
			new() { bestResult = 6, resultMask = 16, resultNumBits = 1 },	//[3][16]=[ 11][ 10000]->{110, 10000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][17]=[ 11][ 10001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][18]=[ 11][ 10010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][19]=[ 11][ 10011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 4, resultNumBits = 1 },	//[3][20]=[ 11][ 10100]->{101,   100,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][21]=[ 11][ 10101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][22]=[ 11][ 10110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][23]=[ 11][ 10111]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 8, resultNumBits = 1 },	//[3][24]=[ 11][ 11000]->{101,  1000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][25]=[ 11][ 11001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][26]=[ 11][ 11010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][27]=[ 11][ 11011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 12, resultNumBits = 2 },	//[3][28]=[ 11][ 11100]->{101,  1100,2}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][29]=[ 11][ 11101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][30]=[ 11][ 11110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][31]=[ 11][ 11111]->{ 11,    11,2}
			new() { bestResult = 6, resultMask = 32, resultNumBits = 1 },	//[3][32]=[ 11][100000]->{110,100000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][33]=[ 11][100001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][34]=[ 11][100010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][35]=[ 11][100011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 4, resultNumBits = 1 },	//[3][36]=[ 11][100100]->{101,   100,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][37]=[ 11][100101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][38]=[ 11][100110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][39]=[ 11][100111]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 8, resultNumBits = 1 },	//[3][40]=[ 11][101000]->{101,  1000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][41]=[ 11][101001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][42]=[ 11][101010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][43]=[ 11][101011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 12, resultNumBits = 2 },	//[3][44]=[ 11][101100]->{101,  1100,2}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][45]=[ 11][101101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][46]=[ 11][101110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][47]=[ 11][101111]->{ 11,    11,2}
			new() { bestResult = 6, resultMask = 48, resultNumBits = 2 },	//[3][48]=[ 11][110000]->{110,110000,2}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][49]=[ 11][110001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][50]=[ 11][110010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][51]=[ 11][110011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 4, resultNumBits = 1 },	//[3][52]=[ 11][110100]->{101,   100,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][53]=[ 11][110101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][54]=[ 11][110110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][55]=[ 11][110111]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 8, resultNumBits = 1 },	//[3][56]=[ 11][111000]->{101,  1000,1}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][57]=[ 11][111001]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][58]=[ 11][111010]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][59]=[ 11][111011]->{ 11,    11,2}
			new() { bestResult = 5, resultMask = 12, resultNumBits = 2 },	//[3][60]=[ 11][111100]->{101,  1100,2}
			new() { bestResult = 3, resultMask = 1, resultNumBits = 1 },	//[3][61]=[ 11][111101]->{ 11,     1,1}
			new() { bestResult = 3, resultMask = 2, resultNumBits = 1 },	//[3][62]=[ 11][111110]->{ 11,    10,1}
			new() { bestResult = 3, resultMask = 3, resultNumBits = 2 },	//[3][63]=[ 11][111111]->{ 11,    11,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[4][0]=[100][     0]->{111,     0,0}
			new() { bestResult = 4, resultMask = 1, resultNumBits = 1 },	//[4][1]=[100][     1]->{100,     1,1}
			new() { bestResult = 4, resultMask = 2, resultNumBits = 1 },	//[4][2]=[100][    10]->{100,    10,1}
			new() { bestResult = 4, resultMask = 3, resultNumBits = 2 },	//[4][3]=[100][    11]->{100,    11,2}
			new() { bestResult = 2, resultMask = 4, resultNumBits = 1 },	//[4][4]=[100][   100]->{ 10,   100,1}
			new() { bestResult = 2, resultMask = 4, resultNumBits = 1 },	//[4][5]=[100][   101]->{ 10,   100,1}
			new() { bestResult = 2, resultMask = 4, resultNumBits = 1 },	//[4][6]=[100][   110]->{ 10,   100,1}
			new() { bestResult = 2, resultMask = 4, resultNumBits = 1 },	//[4][7]=[100][   111]->{ 10,   100,1}
			new() { bestResult = 2, resultMask = 8, resultNumBits = 1 },	//[4][8]=[100][  1000]->{ 10,  1000,1}
			new() { bestResult = 2, resultMask = 8, resultNumBits = 1 },	//[4][9]=[100][  1001]->{ 10,  1000,1}
			new() { bestResult = 2, resultMask = 8, resultNumBits = 1 },	//[4][10]=[100][  1010]->{ 10,  1000,1}
			new() { bestResult = 2, resultMask = 8, resultNumBits = 1 },	//[4][11]=[100][  1011]->{ 10,  1000,1}
			new() { bestResult = 2, resultMask = 12, resultNumBits = 2 },	//[4][12]=[100][  1100]->{ 10,  1100,2}
			new() { bestResult = 2, resultMask = 12, resultNumBits = 2 },	//[4][13]=[100][  1101]->{ 10,  1100,2}
			new() { bestResult = 2, resultMask = 12, resultNumBits = 2 },	//[4][14]=[100][  1110]->{ 10,  1100,2}
			new() { bestResult = 2, resultMask = 12, resultNumBits = 2 },	//[4][15]=[100][  1111]->{ 10,  1100,2}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][16]=[100][ 10000]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][17]=[100][ 10001]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][18]=[100][ 10010]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][19]=[100][ 10011]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][20]=[100][ 10100]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][21]=[100][ 10101]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][22]=[100][ 10110]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][23]=[100][ 10111]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][24]=[100][ 11000]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][25]=[100][ 11001]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][26]=[100][ 11010]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][27]=[100][ 11011]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][28]=[100][ 11100]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][29]=[100][ 11101]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][30]=[100][ 11110]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 16, resultNumBits = 1 },	//[4][31]=[100][ 11111]->{  1, 10000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][32]=[100][100000]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][33]=[100][100001]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][34]=[100][100010]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][35]=[100][100011]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][36]=[100][100100]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][37]=[100][100101]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][38]=[100][100110]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][39]=[100][100111]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][40]=[100][101000]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][41]=[100][101001]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][42]=[100][101010]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][43]=[100][101011]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][44]=[100][101100]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][45]=[100][101101]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][46]=[100][101110]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 32, resultNumBits = 1 },	//[4][47]=[100][101111]->{  1,100000,1}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][48]=[100][110000]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][49]=[100][110001]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][50]=[100][110010]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][51]=[100][110011]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][52]=[100][110100]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][53]=[100][110101]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][54]=[100][110110]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][55]=[100][110111]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][56]=[100][111000]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][57]=[100][111001]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][58]=[100][111010]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][59]=[100][111011]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][60]=[100][111100]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][61]=[100][111101]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][62]=[100][111110]->{  1,110000,2}
			new() { bestResult = 1, resultMask = 48, resultNumBits = 2 },	//[4][63]=[100][111111]->{  1,110000,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[5][0]=[101][     0]->{111,     0,0}
			new() { bestResult = 5, resultMask = 1, resultNumBits = 1 },	//[5][1]=[101][     1]->{101,     1,1}
			new() { bestResult = 6, resultMask = 2, resultNumBits = 1 },	//[5][2]=[101][    10]->{110,    10,1}
			new() { bestResult = 5, resultMask = 1, resultNumBits = 1 },	//[5][3]=[101][    11]->{101,     1,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][4]=[101][   100]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][5]=[101][   101]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][6]=[101][   110]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][7]=[101][   111]->{ 11,   100,1}
			new() { bestResult = 6, resultMask = 8, resultNumBits = 1 },	//[5][8]=[101][  1000]->{110,  1000,1}
			new() { bestResult = 5, resultMask = 1, resultNumBits = 1 },	//[5][9]=[101][  1001]->{101,     1,1}
			new() { bestResult = 6, resultMask = 10, resultNumBits = 2 },	//[5][10]=[101][  1010]->{110,  1010,2}
			new() { bestResult = 5, resultMask = 1, resultNumBits = 1 },	//[5][11]=[101][  1011]->{101,     1,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][12]=[101][  1100]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][13]=[101][  1101]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][14]=[101][  1110]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][15]=[101][  1111]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][16]=[101][ 10000]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][17]=[101][ 10001]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][18]=[101][ 10010]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][19]=[101][ 10011]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][20]=[101][ 10100]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][21]=[101][ 10101]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][22]=[101][ 10110]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][23]=[101][ 10111]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][24]=[101][ 11000]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][25]=[101][ 11001]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][26]=[101][ 11010]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][27]=[101][ 11011]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][28]=[101][ 11100]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][29]=[101][ 11101]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][30]=[101][ 11110]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][31]=[101][ 11111]->{ 11, 10100,2}
			new() { bestResult = 5, resultMask = 32, resultNumBits = 1 },	//[5][32]=[101][100000]->{101,100000,1}
			new() { bestResult = 5, resultMask = 33, resultNumBits = 2 },	//[5][33]=[101][100001]->{101,100001,2}
			new() { bestResult = 5, resultMask = 32, resultNumBits = 1 },	//[5][34]=[101][100010]->{101,100000,1}
			new() { bestResult = 5, resultMask = 33, resultNumBits = 2 },	//[5][35]=[101][100011]->{101,100001,2}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][36]=[101][100100]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][37]=[101][100101]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][38]=[101][100110]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][39]=[101][100111]->{ 11,   100,1}
			new() { bestResult = 5, resultMask = 32, resultNumBits = 1 },	//[5][40]=[101][101000]->{101,100000,1}
			new() { bestResult = 5, resultMask = 33, resultNumBits = 2 },	//[5][41]=[101][101001]->{101,100001,2}
			new() { bestResult = 5, resultMask = 32, resultNumBits = 1 },	//[5][42]=[101][101010]->{101,100000,1}
			new() { bestResult = 5, resultMask = 33, resultNumBits = 2 },	//[5][43]=[101][101011]->{101,100001,2}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][44]=[101][101100]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][45]=[101][101101]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][46]=[101][101110]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 4, resultNumBits = 1 },	//[5][47]=[101][101111]->{ 11,   100,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][48]=[101][110000]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][49]=[101][110001]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][50]=[101][110010]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][51]=[101][110011]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][52]=[101][110100]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][53]=[101][110101]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][54]=[101][110110]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][55]=[101][110111]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][56]=[101][111000]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][57]=[101][111001]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][58]=[101][111010]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 16, resultNumBits = 1 },	//[5][59]=[101][111011]->{ 11, 10000,1}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][60]=[101][111100]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][61]=[101][111101]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][62]=[101][111110]->{ 11, 10100,2}
			new() { bestResult = 3, resultMask = 20, resultNumBits = 2 },	//[5][63]=[101][111111]->{ 11, 10100,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[6][0]=[110][     0]->{111,     0,0}
			new() { bestResult = 6, resultMask = 1, resultNumBits = 1 },	//[6][1]=[110][     1]->{110,     1,1}
			new() { bestResult = 5, resultMask = 2, resultNumBits = 1 },	//[6][2]=[110][    10]->{101,    10,1}
			new() { bestResult = 5, resultMask = 2, resultNumBits = 1 },	//[6][3]=[110][    11]->{101,    10,1}
			new() { bestResult = 6, resultMask = 4, resultNumBits = 1 },	//[6][4]=[110][   100]->{110,   100,1}
			new() { bestResult = 6, resultMask = 5, resultNumBits = 2 },	//[6][5]=[110][   101]->{110,   101,2}
			new() { bestResult = 5, resultMask = 2, resultNumBits = 1 },	//[6][6]=[110][   110]->{101,    10,1}
			new() { bestResult = 5, resultMask = 2, resultNumBits = 1 },	//[6][7]=[110][   111]->{101,    10,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][8]=[110][  1000]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][9]=[110][  1001]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][10]=[110][  1010]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][11]=[110][  1011]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][12]=[110][  1100]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][13]=[110][  1101]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][14]=[110][  1110]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][15]=[110][  1111]->{ 11,  1000,1}
			new() { bestResult = 5, resultMask = 16, resultNumBits = 1 },	//[6][16]=[110][ 10000]->{101, 10000,1}
			new() { bestResult = 5, resultMask = 16, resultNumBits = 1 },	//[6][17]=[110][ 10001]->{101, 10000,1}
			new() { bestResult = 5, resultMask = 18, resultNumBits = 2 },	//[6][18]=[110][ 10010]->{101, 10010,2}
			new() { bestResult = 5, resultMask = 18, resultNumBits = 2 },	//[6][19]=[110][ 10011]->{101, 10010,2}
			new() { bestResult = 5, resultMask = 16, resultNumBits = 1 },	//[6][20]=[110][ 10100]->{101, 10000,1}
			new() { bestResult = 5, resultMask = 16, resultNumBits = 1 },	//[6][21]=[110][ 10101]->{101, 10000,1}
			new() { bestResult = 5, resultMask = 18, resultNumBits = 2 },	//[6][22]=[110][ 10110]->{101, 10010,2}
			new() { bestResult = 5, resultMask = 18, resultNumBits = 2 },	//[6][23]=[110][ 10111]->{101, 10010,2}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][24]=[110][ 11000]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][25]=[110][ 11001]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][26]=[110][ 11010]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][27]=[110][ 11011]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][28]=[110][ 11100]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][29]=[110][ 11101]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][30]=[110][ 11110]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 8, resultNumBits = 1 },	//[6][31]=[110][ 11111]->{ 11,  1000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][32]=[110][100000]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][33]=[110][100001]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][34]=[110][100010]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][35]=[110][100011]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][36]=[110][100100]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][37]=[110][100101]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][38]=[110][100110]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][39]=[110][100111]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][40]=[110][101000]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][41]=[110][101001]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][42]=[110][101010]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][43]=[110][101011]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][44]=[110][101100]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][45]=[110][101101]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][46]=[110][101110]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][47]=[110][101111]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][48]=[110][110000]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][49]=[110][110001]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][50]=[110][110010]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][51]=[110][110011]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][52]=[110][110100]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][53]=[110][110101]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][54]=[110][110110]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 32, resultNumBits = 1 },	//[6][55]=[110][110111]->{ 11,100000,1}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][56]=[110][111000]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][57]=[110][111001]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][58]=[110][111010]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][59]=[110][111011]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][60]=[110][111100]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][61]=[110][111101]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][62]=[110][111110]->{ 11,101000,2}
			new() { bestResult = 3, resultMask = 40, resultNumBits = 2 },	//[6][63]=[110][111111]->{ 11,101000,2}
		},
		{
			new() { bestResult = 65535, resultMask = 0, resultNumBits = 0 },	//[7][0]=[111][     0]->{111,     0,0}
			new() { bestResult = 7, resultMask = 1, resultNumBits = 1 },	//[7][1]=[111][     1]->{111,     1,1}
			new() { bestResult = 7, resultMask = 2, resultNumBits = 1 },	//[7][2]=[111][    10]->{111,    10,1}
			new() { bestResult = 7, resultMask = 3, resultNumBits = 2 },	//[7][3]=[111][    11]->{111,    11,2}
			new() { bestResult = 7, resultMask = 4, resultNumBits = 1 },	//[7][4]=[111][   100]->{111,   100,1}
			new() { bestResult = 7, resultMask = 5, resultNumBits = 2 },	//[7][5]=[111][   101]->{111,   101,2}
			new() { bestResult = 7, resultMask = 6, resultNumBits = 2 },	//[7][6]=[111][   110]->{111,   110,2}
			new() { bestResult = 7, resultMask = 7, resultNumBits = 3 },	//[7][7]=[111][   111]->{111,   111,3}
			new() { bestResult = 7, resultMask = 8, resultNumBits = 1 },	//[7][8]=[111][  1000]->{111,  1000,1}
			new() { bestResult = 7, resultMask = 9, resultNumBits = 2 },	//[7][9]=[111][  1001]->{111,  1001,2}
			new() { bestResult = 7, resultMask = 10, resultNumBits = 2 },	//[7][10]=[111][  1010]->{111,  1010,2}
			new() { bestResult = 7, resultMask = 11, resultNumBits = 3 },	//[7][11]=[111][  1011]->{111,  1011,3}
			new() { bestResult = 7, resultMask = 12, resultNumBits = 2 },	//[7][12]=[111][  1100]->{111,  1100,2}
			new() { bestResult = 7, resultMask = 13, resultNumBits = 3 },	//[7][13]=[111][  1101]->{111,  1101,3}
			new() { bestResult = 7, resultMask = 14, resultNumBits = 3 },	//[7][14]=[111][  1110]->{111,  1110,3}
			new() { bestResult = 7, resultMask = 15, resultNumBits = 4 },	//[7][15]=[111][  1111]->{111,  1111,4}
			new() { bestResult = 7, resultMask = 16, resultNumBits = 1 },	//[7][16]=[111][ 10000]->{111, 10000,1}
			new() { bestResult = 7, resultMask = 17, resultNumBits = 2 },	//[7][17]=[111][ 10001]->{111, 10001,2}
			new() { bestResult = 7, resultMask = 18, resultNumBits = 2 },	//[7][18]=[111][ 10010]->{111, 10010,2}
			new() { bestResult = 7, resultMask = 19, resultNumBits = 3 },	//[7][19]=[111][ 10011]->{111, 10011,3}
			new() { bestResult = 7, resultMask = 20, resultNumBits = 2 },	//[7][20]=[111][ 10100]->{111, 10100,2}
			new() { bestResult = 7, resultMask = 21, resultNumBits = 3 },	//[7][21]=[111][ 10101]->{111, 10101,3}
			new() { bestResult = 7, resultMask = 22, resultNumBits = 3 },	//[7][22]=[111][ 10110]->{111, 10110,3}
			new() { bestResult = 7, resultMask = 23, resultNumBits = 4 },	//[7][23]=[111][ 10111]->{111, 10111,4}
			new() { bestResult = 7, resultMask = 24, resultNumBits = 2 },	//[7][24]=[111][ 11000]->{111, 11000,2}
			new() { bestResult = 7, resultMask = 25, resultNumBits = 3 },	//[7][25]=[111][ 11001]->{111, 11001,3}
			new() { bestResult = 7, resultMask = 26, resultNumBits = 3 },	//[7][26]=[111][ 11010]->{111, 11010,3}
			new() { bestResult = 7, resultMask = 27, resultNumBits = 4 },	//[7][27]=[111][ 11011]->{111, 11011,4}
			new() { bestResult = 7, resultMask = 28, resultNumBits = 3 },	//[7][28]=[111][ 11100]->{111, 11100,3}
			new() { bestResult = 7, resultMask = 29, resultNumBits = 4 },	//[7][29]=[111][ 11101]->{111, 11101,4}
			new() { bestResult = 7, resultMask = 30, resultNumBits = 4 },	//[7][30]=[111][ 11110]->{111, 11110,4}
			new() { bestResult = 7, resultMask = 31, resultNumBits = 5 },	//[7][31]=[111][ 11111]->{111, 11111,5}
			new() { bestResult = 7, resultMask = 32, resultNumBits = 1 },	//[7][32]=[111][100000]->{111,100000,1}
			new() { bestResult = 7, resultMask = 33, resultNumBits = 2 },	//[7][33]=[111][100001]->{111,100001,2}
			new() { bestResult = 7, resultMask = 34, resultNumBits = 2 },	//[7][34]=[111][100010]->{111,100010,2}
			new() { bestResult = 7, resultMask = 35, resultNumBits = 3 },	//[7][35]=[111][100011]->{111,100011,3}
			new() { bestResult = 7, resultMask = 36, resultNumBits = 2 },	//[7][36]=[111][100100]->{111,100100,2}
			new() { bestResult = 7, resultMask = 37, resultNumBits = 3 },	//[7][37]=[111][100101]->{111,100101,3}
			new() { bestResult = 7, resultMask = 38, resultNumBits = 3 },	//[7][38]=[111][100110]->{111,100110,3}
			new() { bestResult = 7, resultMask = 39, resultNumBits = 4 },	//[7][39]=[111][100111]->{111,100111,4}
			new() { bestResult = 7, resultMask = 40, resultNumBits = 2 },	//[7][40]=[111][101000]->{111,101000,2}
			new() { bestResult = 7, resultMask = 41, resultNumBits = 3 },	//[7][41]=[111][101001]->{111,101001,3}
			new() { bestResult = 7, resultMask = 42, resultNumBits = 3 },	//[7][42]=[111][101010]->{111,101010,3}
			new() { bestResult = 7, resultMask = 43, resultNumBits = 4 },	//[7][43]=[111][101011]->{111,101011,4}
			new() { bestResult = 7, resultMask = 44, resultNumBits = 3 },	//[7][44]=[111][101100]->{111,101100,3}
			new() { bestResult = 7, resultMask = 45, resultNumBits = 4 },	//[7][45]=[111][101101]->{111,101101,4}
			new() { bestResult = 7, resultMask = 46, resultNumBits = 4 },	//[7][46]=[111][101110]->{111,101110,4}
			new() { bestResult = 7, resultMask = 47, resultNumBits = 5 },	//[7][47]=[111][101111]->{111,101111,5}
			new() { bestResult = 7, resultMask = 48, resultNumBits = 2 },	//[7][48]=[111][110000]->{111,110000,2}
			new() { bestResult = 7, resultMask = 49, resultNumBits = 3 },	//[7][49]=[111][110001]->{111,110001,3}
			new() { bestResult = 7, resultMask = 50, resultNumBits = 3 },	//[7][50]=[111][110010]->{111,110010,3}
			new() { bestResult = 7, resultMask = 51, resultNumBits = 4 },	//[7][51]=[111][110011]->{111,110011,4}
			new() { bestResult = 7, resultMask = 52, resultNumBits = 3 },	//[7][52]=[111][110100]->{111,110100,3}
			new() { bestResult = 7, resultMask = 53, resultNumBits = 4 },	//[7][53]=[111][110101]->{111,110101,4}
			new() { bestResult = 7, resultMask = 54, resultNumBits = 4 },	//[7][54]=[111][110110]->{111,110110,4}
			new() { bestResult = 7, resultMask = 55, resultNumBits = 5 },	//[7][55]=[111][110111]->{111,110111,5}
			new() { bestResult = 7, resultMask = 56, resultNumBits = 3 },	//[7][56]=[111][111000]->{111,111000,3}
			new() { bestResult = 7, resultMask = 57, resultNumBits = 4 },	//[7][57]=[111][111001]->{111,111001,4}
			new() { bestResult = 7, resultMask = 58, resultNumBits = 4 },	//[7][58]=[111][111010]->{111,111010,4}
			new() { bestResult = 7, resultMask = 59, resultNumBits = 5 },	//[7][59]=[111][111011]->{111,111011,5}
			new() { bestResult = 7, resultMask = 60, resultNumBits = 4 },	//[7][60]=[111][111100]->{111,111100,4}
			new() { bestResult = 7, resultMask = 61, resultNumBits = 5 },	//[7][61]=[111][111101]->{111,111101,5}
			new() { bestResult = 7, resultMask = 62, resultNumBits = 5 },	//[7][62]=[111][111110]->{111,111110,5}
			new() { bestResult = 7, resultMask = 63, resultNumBits = 6 },	//[7][63]=[111][111111]->{111,111111,6}
		},
	};


	public int bestResult;

	public int resultMask;

	public int resultNumBits;
}
