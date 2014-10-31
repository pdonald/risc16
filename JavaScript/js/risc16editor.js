var cpu = new CPU();
var paused = false;

function numeric(n) {
	return {
		bin: zeroFill(n.toString(2), 16),
		dec: n,
		dec2: n & 0x8000 ? n - 0xffff - 1 : n,
		hex: zeroFill(n.toString(16).toUpperCase(), 4),
		asm: instruction2asm(n)
	};
}

function createRegistersTable() {
	for (var i = 0; i < cpu.getRegisters().getCount(); i++) {
		$('#registers').append('<tr id="reg' + i + '"><td>R' + i + '</td><td id="reg' + i + 'bin"></td><td id="reg' + i + 'dec"></td><td id="reg' + i + 'hex"></td></tr>');
	}
}

function updateRegistersTable() {
	for (var i = 0; i < cpu.getRegisters().getCount(); i++) {
		var curr = bits2text(cpu.getRegisters().get(i));
		var prev = $('#reg' + i + 'bin').text();
		
		var bin = curr;
		var dec = parseInt(bin, 2);
		$('#reg' + i + 'bin').text(bin);
		$('#reg' + i + 'bin').attr('title', instruction2asm(dec));
		$('#reg' + i + 'dec').text(dec);
		$('#reg' + i + 'dec').attr('title', dec & 0x8000 ? dec - 0xffff - 1 : '');
		$('#reg' + i + 'hex').text(zeroFill(dec.toString(16).toUpperCase(), 4));
		
		if (prev != "" && prev != curr) {
			$('#reg' + i).animate({ color: 'red' }, 50).animate({ color: '#669' }, 50);
		}
	}
}

var memoryprev;
function updateMemoryTable() {
	$('#memory > tbody').find('tr').remove();

	for (var i = 0; i < cpu.getMemory().getSize(); i++) {
		var x = cpu.getMemory().get2(i);
		if (x != undefined || (bits2int(cpu.getCurrentAddress()) == i)) {
			var val = numeric(parseInt(bits2text(cpu.getMemory().get(i)), 2));
			var addr = numeric(i);
			var addr_title = addr.bin + ' / ' + addr.dec + (addr.dec != addr.dec2 ? ' (' + addr.dec2 + ')' : '') + ' / ' + addr.asm;
			
			$('#memory').append('<tr><td>' + 
				(!cpu.isHalted() && bits2int(cpu.getCurrentAddress()) == i ? '<img src="images/step.png" alt="Nākamā instrukcija" title="Nākamā instrukcija" />' : '') +
				(memoryprev == i ? '<img src="images/current.png" alt="Izpildītā instrukcija" title="Tikko izpildītā instrukcija" />' : '') +
				 '</td><td title="' + addr_title + '">' + addr.hex + '</td><td title="' + val.asm + '">' + val.bin + '</td><td title="' + (val.dec != val.dec2 ? val.dec2 : '') + '">' + val.dec + '</td><td>' + val.hex + '</td></tr>');
		}
	}
	
	memoryprev = bits2int(cpu.getCurrentAddress());
}

function compile(translator) {
	cpu.reset();
	cpu.getMemory().load(translator(editor.getCode()));
	memoryprev = -1;
	updateMemoryTable();
	// if not any errors

	enable(false, 'suspend'); enable(true, 'debug', 'run', 'reset', 'step');
}

function debug() {
	if (!cpu.isHalted() && !paused) {
		nextStep();
		setTimeout("debug()", 1);
	} else {
		debug_end();
	}
}

var runit = 0;
function run() {
	while (!cpu.isHalted() && !paused) {
		cpu.nextStep();
		if (runit++ >= 5000) { // ja vislaik pauzēs kā zemāk, tad viss izpildīsies ļoti ļoti lēnu
			runit = 0;
			setTimeout("run()", 1); // jāiepauzē, lai nerastos iespaids, ka lapa ir nokārusies
			return;
		}
	}

	run_end();
}

function nextStep() {
	cpu.nextStep();
	update();
}

function update() {
	updateRegistersTable();
	updateMemoryTable();

	if (paused) {
		pause_end();
	}

	if (cpu.isHalted()) {
		enable(true, 'reset');
		enable(false, 'debug', 'run', 'suspend', 'step');
	}
}

function enable(enabled) {
	for (var i = 1; i < arguments.length; i++) {
		if (!enabled) $('#' + arguments[i]).addClass('disabled');
		else $('#' + arguments[i]).removeClass('disabled');
	}
}

function debug_start() { enable(true, 'suspend'); enable(false, 'debug', 'run', 'reset', 'step'); debug(); }
function debug_end() { enable(false, 'suspend'); enable(true, 'debug', 'run', 'reset', 'step'); update(); }
function run_start() { enable(true, 'suspend'); enable(false, 'debug', 'run', 'reset', 'step'); run(); }
function run_end() { enable(false, 'suspend'); enable(true, 'debug', 'run', 'reset', 'step'); update(); }
function pause_start() { paused = true; }
function pause_end() { paused = false; enable(true, 'debug', 'run', 'reset', 'step'); enable(false, 'suspend'); update(); }
function reset() { enable(true, 'debug', 'run', 'reset', 'step'); enable(false, 'suspend'); cpu.reset(); memoryprev = -1; update();  }

$(document).ready(function(){
	tabs();

	$('#debug').click(function() { debug_start(); });
	$('#run').click(function() { run_start(); });
	$('#step').click(function() { nextStep(); });
	$('#suspend').click(function() { pause_start(); });
	$('#reset').click(function() { reset(); });
	$('#compile_asm').click(function() { compile(asm2opcodeTranslator); });
	$('#compile_opcode').click(function() { compile(opcode2opcodeTranslator); });

	createRegistersTable();
	updateRegistersTable();
	updateMemoryTable();
	
	enable(false, 'debug', 'run', 'reset', 'step', 'suspend');
});