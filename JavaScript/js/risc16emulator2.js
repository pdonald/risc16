

function CPU() {
	// nav enkapsulācijas ātrdarbības dēļ - lai nebūtu lieki n-tie funkciju izsaukumi pēc katras darbības
	this.registers = [ 0, 0, 0, 0, 0, 0, 0, 0 ];
	this.memory = [];
	this.instructionPointer = 0;
	this.halted = false;
	
	this.reset = function() {
		instructionPointer = 0;
		halted = false;
	}
	
	this.step = function() {
		if (!halted) {
			execute(memory[instructionPointer]);
		}
	}
	
	function execute(instruction) {
		var opcode = instruction & 0xE000;
		var regA = (instruction & 0x1C00) >> 10;
		var regB = (instruction & 0x380) >> 7;
		var regC = instruction & 0x0007;
		var imm7 = instruction & 0x007f; if (imm7 & 0x0040 != 0) imm7 |= 0xFFC0;
		var imm10 = instruction & 0x03ff;
		var imm4 = instruction & 0x0078;
		
		switch (opcode) {
			case "000":
			case "010":
				
				
				switch (opcode) {
					case "000": add(regA, regB, regC); break;
					case "010": nand(regA, regB, regC); break;
				}
				
				break;
			
			case "001":
			case "101":
			case "100":
			case "110":
			case "111":
				// expand
                var imm = parseInt(instruction.substring(9, 16), 2);
                
                switch (opcode) {
					case "001": addi(regA, regB, imm); break;
					case "101": sw(regA, regB, imm); break;
					case "100": lw(regA, regB, imm); break;
					case "110": beq(regA, regB, imm); break;
					case "111": if (imm != 0) halted = true; else jalr(regA, regB); break;
				}
				
				break;
			
			case "011":
				var imm = parseInt(instruction.substring(6, 16), 2);
				lui(regA, imm);
				break;
		}
		
		if (!halted) {
			currentAddress = sum(currentAddress, 1);
		}
	}
	
	function add(regA, regB, regC) {
		if (regA != 0) {
			registers[regA] = sum(registers[regB], registers[regC]);
		}
	}
	
	function addi(regA, regB, imm) {
		if (regA != 0) {
			registers[regA] = sum(registers[regB], imm);
		}
	}
	
	function nand(regA, regB, regC) {
		if (regA != 0) {
			registers[regA] = ~(registers[regB] & registers[regC]);
		}
	}
	
	function lui(regA, imm) {
		if (regA != 0) {
			registers[regA] |= imm << 6;
		}
	}
	
	function sw(regA, regB, imm) {
		var address = sum(registers[regB], imm);
		memory[address] = registers[regA];
	}
	
	function lw(regA, regB, imm) {
		if (regA != 0) {
			var address = sum(registers[regB], imm);
			registers[regA] = memory[address];
		}
	}
	
	function beq(regA, regB, imm) {
		if (registers[regA] == registers[regB]) {
			currentAddress = sum(currentAddress, imm); // iekš execute jau ir +1
		}
	}
	
	function jalr(regA, regB) {
		if (regA != 0) {
			registers[regA] = sum(currentAddress, 1);
		}
		currentAddress = registers[regB];
	}
	
	function sum(a, b) {
		return (a + b) % (0xffff + 1);
	}
}

function bits2text(bits) {
	var s = "";
	for (var i = 0; i < bits.length; i++)
		s += bits[i] ? "1" : "0";
	return s;
}

function text2bits(text) {
	var bits = new Array(text.length);
	for (var i = 0; i < text.length; i++)
		bits[i] = text[i] == "1";
	return bits;
}

function bits2int(bits, signed) {
	if (signed == undefined || !signed) {
		return parseInt(bits2text(bits), 2);
	} else {
		// -1 * 
	}
}

function int2bits(n, length) {
	return text2bits(zeroFill(parseInt(n).toString(2), length));
}

function zeroFill( number, width ) { // iz http://stackoverflow.com/questions/1267283/how-can-i-create-a-zerofilled-value-using-javascript
	width -= number.toString().length;
	if ( width > 0 ) {
		return new Array( width + (/\./.test( number ) ? 2 : 1) ).join( '0' ) + number;
	}
	return number;
}