
// nepārbauda argumentu apgabalu
// masīvs ar bināru skaitli
// binārs cipars
// reg var būt vai nu skaitlis vai arī masīvs ar bināru skaitli, kas tiek pārveidots par skaitli
// reg jābūt 0 <= reg < getCount()
// reg vērtības apgabals netiek pārbaudīts
function Registers() {

	// 2D masīvs, kurā glabājas reģistri un tajos esošie vārdi
	var registers;

	// reg pārvērš par reģistra numuru
	// ja reg ir skaitlis, reģistra numurs ir šis skaitlis
	// ja reg ir vārds, tad tas tiek pārvērsts par skaitli
	function getReg(reg) {
		return (reg instanceof Array ? bits2int(reg) : reg);
	}

	// vārds, kas glabājas reģistrā ar numuru reg
	this.get = function(reg) {
		if (registers[getReg(reg)] == undefined) alert(getReg(reg));
		return registers[getReg(reg)].slice(0); // kopija
	};
	
	// reģistrā ar numuru reg ieraksta vārdu value
	// ja reg = 0, tad reģistra vērtība nemainās un paliek 0
	this.set = function(reg, value) {
		reg = getReg(reg);
		if (reg != 0) {
			for (var i = 0; i < value.length; i++) {
				this.setBit(reg, i, value[i]);
			}
		}
	};
	
	// vārda bit-ais bits, kas glabājas reģistrā ar numuru reg
	this.getBit = function(reg, bit) {
		return this.get(reg)[bit];
	}
	
	// reģistra ar numuru reg vārda bit bitā ieraksta bitu value
	// value tiks pārvērsta par bitu
	// ja reg = 0, tad reģistra vērtība neatkarīgi no bit nemainās un paliek 0
	this.setBit = function(reg, bit, value) {
		reg = getReg(reg);
		if (reg != 0) {
			registers[reg][bit] = value ? true : false;
		}
	}
	
	// cik kopā reģistru
	this.getCount = function() { return 8; };
	
	// katra reģistra vārda garums
	this.getWordLength = function() { return 16; };
	
	// aizpilda visos reģistros vārdus ar 0
	this.reset = function() {
		registers = new Array(this.getCount());
		for (var i = 0; i < this.getCount(); i++) {
			registers[i] = new Array(this.getWordLength());
			for (var j = 0; j < this.getWordLength(); j++) {
				registers[i][j] = false;
			}
		}
	}
	
	this.reset();
}

function Memory() {
	
	var memory = [];
	
	function getAddress(address) {
		return (address instanceof Array ? bits2int(address) : address);
	}
	
	this.get = function(address) {
		address = getAddress(address);
		if (memory[address] == undefined) {
			this.set(address, int2bits(0, this.getWordLength()));
		}
		return memory[address].slice(0); // kopija
	}
	
	this.get2 = function(address) {
		return memory[address] != undefined ? memory[address] : undefined;
	}
	
	this.set = function(address, value) {
		address = getAddress(address);
		memory[address] = new Array(this.getWordLength());
		for (var i = 0; i < value.length; i++) {
			memory[address][i] = value[i] ? true : false;
		}
	}
	
	this.load = function(dump) {
		memory = dump;
	}
	
	this.dump = function() {
		return memory.slice(0);
	}
	
	this.getSize = function() { return 0xffff + 1; };
	this.getWordLength = function() { return 16; };
}

function CPU() {
	var registers = new Registers();
	var memory = new Memory();
	var currentAddress;
	var halted;
	
	// padara ārpasaulei pieejamus reģistrus
	this.getRegisters = function() { return registers; }
	
	// dod ārpasaulei pieeju atmiņai
	this.getMemory = function() { return memory; }
	
	this.getCurrentAddress = function() { return currentAddress; }
	
	this.isHalted = function() { return halted; }
	
	this.reset = function() { currentAddress = int2bits(0, registers.getWordLength()); halted = false; }
	this.reset();
	
	this.nextStep = function() {
		if (!halted) {
			execute(memory.get(currentAddress));
		}
	}
	
	function execute(instruction) {
		var opcode = bits2text(instruction.slice(0, 3));
		
		var regA = instruction.slice(3, 6);
		var regB = instruction.slice(6, 9);
		
		switch (opcode) {
			case "000":
			case "010":
				var regC = instruction.slice(13, 16);
				
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
                var imm = expandSigned(instruction.slice(9, 16), 16);
                
                switch (opcode) {
					case "001": addi(regA, regB, imm); break;
					case "101": sw(regA, regB, imm); break;
					case "100": lw(regA, regB, imm); break;
					case "110": beq(regA, regB, imm); break;
					case "111": jalr(regA, regB, imm); break;
				}
				
				break;
			
			case "011":
				var imm = instruction.slice(6, 16);
				lui(regA, imm);
				break;
		}
		
		if (!halted) {
			currentAddress = binary_add(currentAddress, int2bits(1, registers.getWordLength()));
		}
	}
	
	// regA, regB, regC - reģistru numuri kā vārdi
	function add(regA, regB, regC) {
		addi(regA, regB, registers.get(regC));
	}
	
	// regA, regB - reģistru numuri kā vārdi
	// imm - pieskaitāmais skaitlis kā vārds, kas ir vienādā garumā ar reģistra vārda garumu
	function addi(regA, regB, imm) {
		registers.set(regA, binary_add(registers.get(regB), imm));
	}
	
	// regA, regB, regC - reģistru numuri kā vārdi
	function nand(regA, regB, regC) {
		for (var i = 0; i < registers.getWordLength(); i++) {
			registers.setBit(regA, i, !(registers.getBit(regB, i) && registers.getBit(regC, i)));
		}
	}
	
	// regA - reģistru numurs kā vārds
	// imm - vārds vajadzīgajā garumā (10 biti)
	function lui(regA, imm) {
		for (var i = 0; i < imm.length; i++) {
			registers.setBit(regA, i, imm[i]);
		}
	}
	
	// regA, regB - reģistru numuri kā vārdi
	// imm - pieskaitāmais skaitlis kā vārds, kas ir vienādā garumā ar reģistra vārda garumu
	function sw(regA, regB, imm) {
		var address = binary_add(registers.get(regB), imm);
		memory.set(address, registers.get(regA));
	}
	
	// regA, regB - reģistru numuri kā vārdi
	// imm - pieskaitāmais skaitlis kā vārds, kas ir vienādā garumā ar reģistra vārda garumu
	function lw(regA, regB, imm) {
		var address = binary_add(registers.get(regB), imm);
		registers.set(regA, memory.get(address));
	}
	
	// regA, regB - reģistru numuri kā vārdi
	// imm - pieskaitāmais skaitlis kā vārds, kas ir vienādā garumā ar reģistra vārda garumu
	function beq(regA, regB, imm) {
		for (var i = 0; i < registers.getWordLength(); i++) {
			if (registers.getBit(regA, i) != registers.getBit(regB, i)) {
				return;
			}
		}
		
		currentAddress = binary_add(currentAddress, imm); // iekš execute jau ir +1
	}
	
	// regA, regB - reģistru numuri kā vārdi
	// imm - vārds, kas seko brīvajā vietā, vajadzīgajā garumā (7 biti)
	function jalr(regA, regB, imm) {
		for (var i = 0; i < imm.length; i++) {
			if (imm[i]) {
				halted = true;
				return;
			}
		}
		
		registers.set(regA, binary_add(currentAddress, int2bits(1, registers.getWordLength())));
		currentAddress = registers.get(regB);
	}
	
	// saskaita divus vārdus a un b (tāpat kā uz papīra)
	// abiem vārdu garumiem jābūt vienādiem, taču tas netiek pārbaudīts
	// atgrieztā vārda garums ir vienāds ar a un b vārdu garumiem
	//   bits, kas paliek pāri, tiek atmests
	function binary_add(a, b) {
		var c = new Array(a.length);
		var carry = false;
		
		for (var i = c.length - 1; i >= 0; i--) {
			c[i] = ((a[i] == b[i]) ? carry : !carry);
			carry = ((a[i] == b[i]) ? (a[i] && b[i]) : carry);
		}

		return c;
	}
	
	// paplašina vārdu bits līdz length garumam, ņemot vērā zīmes bitu
	// t.i. pielikto bitu vietā būs zīmes bits
	function expandSigned(bits, length) {
		var expanded = new Array(length);
		
		for (var i = 0; i < length - bits.length; i++)
			expanded[i] = bits[0];
		
		for (var i = 0; i < bits.length; i++)
			expanded[i + (length - bits.length)] = bits[i];
		
		return expanded;
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