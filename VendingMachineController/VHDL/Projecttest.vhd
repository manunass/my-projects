library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;


entity finaltest is
end finaltest;

architecture finaltestarch of finaltest is
component item_select
port(
	inbill, clr : in std_logic_vector(4 downto 0);
	en, clk : in std_logic;
	row: in std_logic_vector(2 downto 0);
	column: in std_logic_vector(3 downto 0);
	choice: in std_logic_vector(1 downto 0);	--What to do if there isn't enough change
	B5000, B1000, C500, C250 : in std_logic_vector(3 downto 0);
	collect: out std_logic_vector(1 to 72);
	extra1, extra2 : out std_logic;
	extra3 : out std_logic_vector(4 downto 0)
);
end component;


signal inbill, clr, extra3 : std_logic_vector(4 downto 0);
signal en, money , change: std_logic;
signal row: std_logic_vector(2 downto 0);
signal column: std_logic_vector(3 downto 0);
signal choice: std_logic_vector(1 downto 0);	--What to do if there isn't enough change
signal collect: std_logic_vector(1 to 72);
signal clk : std_logic := '0'; 



begin 
a1 : item_select port map (inbill , clr , en , clk , row, column , choice , "0001" , "0001" , "0001" , "0001" , collect, money , change, extra3);
clk <= not clk after 5 ns;

process
begin
wait for 0 ns;
clr <= "11111"; en <= '1' ;
wait for 0 ns;
clr <= "00000" ; en <= '1' ; inbill <= "00100" ;
wait for 10 ns;
row <= "001" ; column <= "0001" ; choice <= "00";
wait;
end process;
end finaltestarch;





