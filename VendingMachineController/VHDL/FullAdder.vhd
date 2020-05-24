library IEEE;
use ieee.std_logic_1164.all;

entity fulladder is 
port(
	A, B, Cin : in std_logic;
	O, Cout: out std_logic
);
end fulladder;

architecture fulladderarch of fulladder is 

signal m: std_logic;
begin 

m <= A xor B;
O <=m xor Cin;
Cout <= (A and B) or (Cin and m);

end fulladderarch;