
// pârveido maðînkodu (11100001111000) mûsu procesoram saprotamos vârdos (kâ bool masîvus)
function opcode2opcodeTranslator(input) {
	var output = [];
	var lines = input.split(/\n/);
	
	for (var i in lines) {
		if (lines[i].match(/^[01]{16}$/)) { // ja rindiòa sastâv tikai no 16 vieniniekiem vai nullîtçm
			output.push(text2bits(lines[i])); // pârvçrð par masîvu un pievieno
		}
	}
	
	// TODO: vairâk rindiòu kâ atmiòas
	// TODO: brîdinât par nederîgajâm rindiòâm
	
	return output;
}

function asm2opcodeTranslator(input) {
	function text2opcode(text) {
		switch (text.toLowerCase()) {
			case "add": return "000";
			case "addi": return "001";
			case "nand": return "010";
			case "lui": return "011";
			case "lw": return "100";
			case "sw": return "101";
			case "beq": return "110";
			case "jalr": return "111";
		}
	}
	
	function text2constant(text, length) {
		var n = parseInt(text);
		if (n < 0) n += Math.pow(2, length);
		return bits2text(int2bits(n, length));
	}
	
	var whitespace = '[ \\t]'; // tukðumsimboli - atstarpe vai tab
	var delimiter = '(' + whitespace + '*,' + whitespace + '*|' + whitespace + '+)'; // parametru atdalîtâjsimboli - vai nu viens komats vai viens vai vairâki tukðumsimboli
	var register = '(r?([0-7]))'; // reìistra nosaukums - var sâkties vai nesâkties ar lielo vai mazo r un seko cipars no 0 lîdz 7
	var number = '([a-f0-9-+x]+)'; // skaitlis
	var rrrInstruction = '^(add|nand)' + whitespace + '+' + register + delimiter + register + delimiter + register + '$';
	var rriInstruction = '^(addi|sw|lw|beq|jalr)' + whitespace + '+' + register + delimiter + register + delimiter + number + '$';
	var riInstruction = '^(lui)' + whitespace + '+' + register + delimiter + number + '$';
	
	var instructions = [];
	var lines = input.split(/\n/g);
	
	for (var i in lines) {
		var line = $.trim(lines[i]);
		if (line == '') continue;
		
		var match;
		var instruction = "";
		
		if (match = new RegExp(rrrInstruction, 'i').exec(line)) {
			var opcode = text2opcode(match[1]);
			var r1 = bits2text(int2bits(match[3], 3));
			var r2 = bits2text(int2bits(match[6], 3));
			var r3 = bits2text(int2bits(match[9], 3));
			instruction = opcode + r1 + r2 + "0000" + r3;
		} else if (match = new RegExp(rriInstruction, 'i').exec(line)) {
			var opcode = text2opcode(match[1]);
			var r1 = bits2text(int2bits(match[3], 3));
			var r2 = bits2text(int2bits(match[6], 3));
			var c = text2constant(match[8], 7);
			instruction = opcode + r1 + r2 + c;
		} else if (match = new RegExp(riInstruction, 'i').exec(line)) {
			var opcode = text2opcode(match[1]);
			var r1 = bits2text(int2bits(match[3], 3));
			var c = text2constant(match[5], 10);
			instruction = opcode + r1 + c;
		} else if (line == '.halt') {
			instruction = "1111111111111111";
		}
		
		if (instruction != "")
			instructions.push(instruction);
	}
	
	return opcode2opcodeTranslator(instructions.join('\n'));
}

function instruction2asm(instruction) {
	var asm;
	var opcode = (instruction & 0xE000) >> 13;
	var regA = (instruction & 0x1C00) >> 10;
	var regB = (instruction & 0x380) >> 7;
	var regC = instruction & 0x0007;
	
	var imm7 = (instruction & 0x007f).toString(16).toUpperCase();
	var imm10 = (instruction & 0x03ff).toString(16).toUpperCase();
	var imm4 = (instruction & 0x0078) >> 3;
	
	switch (opcode) {
		case 0: asm = "ADD R" + regA + ", R" + regB + ", R" + regC; break;
		case 1: asm = "ADDI R" + regA + ", R" + regB + ", 0x" + imm7; break;
		case 2: asm = "NAND R" + regA + ", R" + regB + ", R" + regC; break;
		case 3: asm = "LUI R" + regA + ", 0x" + imm10.toString(16); break;
		case 4: asm = "LW R" + regA + ", R" + regB + ", 0x" + imm7; break;
		case 5: asm = "SW R" + regA + ", R" + regB + ", 0x" + imm7; break;
		case 6: asm = "BEQ R" + regA + ", R" + regB + ", 0x" + imm7; break;
		case 7: asm = "JALR R" + regA + ", R" + regB; break;
	}
	
	if (((opcode == 0 || opcode == 2) && imm4 != 0) || (opcode == 7 && imm7 != 0)) {
		asm = "HALT (" + asm + ") / .FILL 0x" + instruction.toString(16).toUpperCase();
	} else if (opcode == 0 && regA == 0 && regB == 0 && regC == 0 && imm4 == 0) {
		asm = "NOP / " + asm;
	}
	
	return asm;
}