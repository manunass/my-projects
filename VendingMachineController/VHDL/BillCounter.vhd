library IEEE;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;
use ieee.std_logic_unsigned.all;

entity Billcounter is
port(
	B5000, B1000, C500 , C250 : in std_logic_vector(3 downto 0);
	lastbill, enough : in std_logic_vector(4 downto 0);
	enoughout : out std_logic;
	B5000back , B1000back, C500back, C250back : out std_logic_vector(3 downto 0)
);
end Billcounter;

architecture Billcounterarch of Billcounter is 
signal Bill5000, Bill1000, Coin500, Coin250, B, a, a1, s, s1, d, d1 : std_logic_vector( 3 downto 0);
signal six1, six2 : std_logic_vector(5 downto 0);
signal seven : std_logic_vector(6 downto 0);
signal u, v, w, x, y, z, y1, z1, k, l, m, n, o: std_logic_vector(4 downto 0);



begin 
B <= "0001";
u <= "10100";
w <= "00100";
y <= "00010";
y1 <= "00001";
Bill5000 <= B5000 ;
Bill1000 <= B1000 ;
Coin500 <= C500;
Coin250 <= C250;

Bill5000 <= std_logic_vector(unsigned(B5000) + unsigned(B) ) when lastbill = "10100" 
else B5000;

Bill1000 <=  std_logic_vector(unsigned(B1000) + unsigned(B) ) when lastbill = "00100" 
else B1000;

Coin500 <=  std_logic_vector(unsigned(C500) + unsigned(B) ) when lastbill = "00010" 
else C500;

Coin250 <=  std_logic_vector(unsigned(C250) + unsigned(B) ) when lastbill = "00001" 
else C250;

v <= std_logic_vector(unsigned(enough) mod unsigned(u)) ;
k <= std_logic_vector(unsigned(enough) / unsigned(u));



x <= std_logic_vector(unsigned(v) mod unsigned(w)) ;
l <= std_logic_vector(unsigned(v) / unsigned(w));
s <= std_logic_vector(unsigned(l(3 downto 0)) - unsigned(Bill1000));
s1(2 downto 1) <= s(1 downto 0);
s1(0) <= '0';
s1(3) <= s(3);

z <= std_logic_vector(unsigned(x) mod unsigned(y)) ;
m <= std_logic_vector(unsigned(x) / unsigned(y));
a <= std_logic_vector(signed(s1) + signed(m(3 downto 0)) - signed(Coin500)) when s(3) = '0'
else std_logic_vector(unsigned(m(3 downto 0)) - unsigned(Coin500));
a1(2 downto 1) <= a(1 downto 0);
a1(0) <= '0';
a1(3) <= s(3);

z1 <= std_logic_vector(unsigned(z) mod unsigned(y1)) ;
n <= std_logic_vector(unsigned(z) / unsigned(y1));
d <= std_logic_vector(signed(a1) + signed(n(3 downto 0)) - signed(Coin250)) when a(3) = '0'
else std_logic_vector(unsigned(n(3 downto 0)) - unsigned(Coin250)) ;


d1 <= std_logic_vector(signed(d) - signed(y1(3 downto 0)));



enoughout <= '1' when d1(3) = '1'
else '0';

B5000back <= "0001" when v = "00000" 
else "0000";

B1000back <= Bill1000 when s(3) = '0'
else l(3 downto 0);

C500back <= Coin500 when a(3) = '0' 
else std_logic_vector(unsigned(a) + unsigned(coin500));

C250back <= Coin250 when d(3) = '0' 
else std_logic_vector(unsigned(d) + unsigned(coin250));





end Billcounterarch;