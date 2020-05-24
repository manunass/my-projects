library IEEE;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity billadder is
port(
	inbill, clr : in std_logic_vector(4 downto 0);
	en, clk : in std_logic;
	total, lastbill : out std_logic_vector(4 downto 0);
	outen : out std_logic
);
end billadder;

architecture billadderarch of billadder is
component DFF
port (CLK, CLR, D: in std_logic;
Q, QN : out std_logic);
end component;

component adder5bit
port(
	A,B : in std_logic_vector(4 downto 0);
	Cin0 : in std_logic ;
	S : out std_logic_vector(4 downto 0);
	Cout : out std_logic
);

end component;

signal Cout : std_logic;


signal d, qn,extra, enabled, bigbill : std_logic_vector(4 downto 0);
signal six : std_logic_vector(5 downto 0);
signal q : std_logic_vector(4 downto 0);

begin 

u0 : DFF port map (clk, clr(0) , d(0), q(0) , qn(0) );
u1 : DFF port map (clk, clr(1) , d(1), q(1) , qn(1) );
u2 : DFF port map (clk, clr(2) , d(2), q(2) , qn(2) );
u3 : DFF port map (clk, clr(3) , d(3), q(3) , qn(3) );
u4 : DFF port map (clk, clr(4) , d(4), q(4) , qn(4) );
a1 : adder5bit port map (q, bigbill, '0' , six(4 downto 0), six(5));



enabled(0) <= en;
enabled(1) <= en;
enabled(2) <= en;
enabled(3) <= en;
enabled(4) <= en;
bigbill <= enabled and inbill;


extra <= six(4 downto 0) when six < "010101"
else q;

lastbill <= (inbill and enabled) when six < "010101"
else "00000";

outen <= en;
d <= extra;
total <= q;

end billadderarch;
