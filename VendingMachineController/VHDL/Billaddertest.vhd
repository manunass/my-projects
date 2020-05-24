library IEEE;
use ieee.std_logic_1164.all;

entity billaddertest is
end billaddertest;


architecture billaddertestarch of billaddertest is
component billadder 
port(
	
	inbill, clr : in std_logic_vector(4 downto 0);
	en, clk : in std_logic;
	total, lastbill : out std_logic_vector(4 downto 0);
	outen : out std_logic
);
end component;

signal inbill,total, clr, lastbill : std_logic_vector(4 downto 0);
signal en, outen :std_logic;
signal clk : std_logic := '0'; 



begin 
a2 : billadder port map (inbill , clr , en , clk, total, lastbill, outen);
clk <= not clk after 5 ns;
process 
begin 
wait for 0 ns;
clr <= "11111";
wait for 0 ns;
clr <= "00000";
inbill <= "00010" ; en <= '1' ;
wait for 10 ns;
inbill <= "00001" ; en <= '0' ;
wait for 10 ns;
inbill <= "00010" ; en <= '1' ;
wait for 10 ns;
inbill <= "00010" ; en <= '1' ;
wait for 10 ns;
inbill <= "00100" ; en <= '1' ;
wait for 10 ns;
inbill <= "01000" ; en <= '0' ;
wait for 10 ns;
inbill <= "10010" ; en <= '1' ;
wait for 10 ns;
inbill <= "10010" ; en <= '1' ;
wait;


end process;
end billaddertestarch;

