library IEEE;
use ieee.std_logic_1164.all;

entity adder5bit is
port(
	A,B : in std_logic_vector(4 downto 0);
	Cin0 : in std_logic ;
	S : out std_logic_vector(4 downto 0);
	Cout : out std_logic
);

end adder5bit;

architecture adderarch of adder5bit is
component fulladder
port(
	A, B, Cin : in std_logic;
	O, Cout: out std_logic
);
end component;


signal Coutvector: std_logic_vector(0 to 5);

begin 

g1 : for i in 0 to 4 generate
U : fulladder port map (A(i) , B(i) , Coutvector(i) , S(i) , Coutvector(i+1) );
end generate;

Coutvector(0) <= Cin0;
Cout <= Coutvector(5);

end adderarch;

