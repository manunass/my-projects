library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity item_select is
port(
	inbill, clr : in std_logic_vector(4 downto 0);
	en, clk : in std_logic;
	row: in std_logic_vector(2 downto 0);
	column: in std_logic_vector(3 downto 0);
	choice: in std_logic_vector(1 downto 0);	--What to do if there isn't enough change
	B5000, B1000, C500, C250 : in std_logic_vector(3 downto 0);
	collect: out std_logic_vector(1 to 72);
	extra1, extra2 : out std_logic;
	extra3: out std_logic_vector(4 downto 0)
);
end item_select;

architecture selectarch of item_select is

component billadder
port(
	inbill, clr : in std_logic_vector(4 downto 0);
	en, clk : in std_logic;
	total, lastbill : out std_logic_vector(4 downto 0);
	outen: out std_logic
);
end component;

component get_value
port (
        clk : in std_logic;
	collect: in std_logic_vector(1 to 72);
	price : out std_logic_vector(4 downto 0)
);
end component;

component Billcounter
port(
	B5000, B1000, C500, C250 : in std_logic_vector(3 downto 0);
	lastbill, enough : in std_logic_vector(4 downto 0);
	enoughout : out std_logic
);
end component;
signal row_ctrl: std_logic_vector(1 to 6) := (OTHERS => '0');
signal collect_ctrl: std_logic_vector(1 to 72) := (OTHERS => '0');
signal mb: std_logic := '0';	--Money back
signal money, change, outen: std_logic;
signal total, lastbill, price, change_needed: std_logic_vector(4 downto 0);

begin
	cash: billadder port map(inbill, clr, en, clk, total, lastbill, outen);
	cost: get_value port map(clk , collect_ctrl, price);
	canreturn: Billcounter port map(B5000, B1000, C500, C250, lastbill, change_needed, change);
extra1 <= money;
extra2 <= change;

	Su: process(row, column, choice, clk)
	begin
	
	case row is					--3-to-8 decoder, whose output is used as enable for 6 4-to-16 decoders
		when "001" => row_ctrl(1) <= '1';
		when "010" => row_ctrl(2) <= '1';
		when "011" => row_ctrl(3) <= '1';
		when "100" => row_ctrl(4) <= '1';
		when "101" => row_ctrl(5) <= '1';
		when "110" => row_ctrl(6) <= '1';
		when OTHERS => row_ctrl <= (OTHERS => '0');
	end case;
	if row_ctrl /= "000000" then
	case column is
		when "0001" => collect_ctrl(1+12*(to_integer(unsigned(row))-1)) <= '1';
		when "0010" => collect_ctrl(2 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "0011" => collect_ctrl(3 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "0100" => collect_ctrl(4 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "0101" => collect_ctrl(5 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "0110" => collect_ctrl(6 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "0111" => collect_ctrl(7 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "1000" => collect_ctrl(8 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "1001" => collect_ctrl(9 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "1010" => collect_ctrl(10 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "1011" => collect_ctrl(11 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when "1100" => collect_ctrl(12 + 12*(to_integer(unsigned(row))-1)) <= '1';
		when OTHERS => collect_ctrl <= (OTHERS => '0');
	end case;
	end if;		--Checking if they selected a missing item
	if (collect_ctrl(7) = '1' or collect_ctrl(8) = '1' or collect_ctrl(9) = '1' or collect_ctrl(10) = '1' or collect_ctrl(11) = '1' or collect_ctrl(12) = '1' or collect_ctrl(19) = '1' or collect_ctrl(20) = '1' or collect_ctrl(21) = '1' or collect_ctrl(22) = '1' or collect_ctrl(23) = '1' or collect_ctrl(24) = '1' or collect_ctrl(31) = '1' or collect_ctrl(32) = '1' or collect_ctrl(33) = '1' or collect_ctrl(34) = '1' or collect_ctrl(35) = '1' or collect_ctrl(36) = '1' or collect_ctrl(59) = '1' or collect_ctrl(60) = '1' or collect_ctrl(71) = '1' or collect_ctrl(72) = '1') then
		mb <= '1';
		change_needed <= total;
	end if;
	if unsigned(price) > unsigned(total) then
		money <= '0';
	else money <= '1';
	end if;
	if change = '0' and money = '1' then		--What to do if we don't have enough change for what they selected
		if choice = "00" then		--Abort
			mb <= '1';
			change_needed <= total;
		elsif choice = "01" then	--Neglect
			mb <= '1';
			change_needed <= "00000";
		elsif choice = "10" then	--Extra Item(s) + Change
			mb <= '0';
			total <= std_logic_vector(unsigned(total) - unsigned(price));
		elsif choice = "11" then	--Extra Item(s) + Neglect Change
			mb <= '0';
			change_needed <= "00000";
			total <= std_logic_vector(unsigned(total) - unsigned(price));
		end if;
	end if;
	
	if money = '0' then				--Checking if they inserted enough money
		mb <= '1';
		change_needed <= total;
	end if;						--Checking if they selected a valid item
	if (unsigned(row_ctrl) = 0 or unsigned(collect_ctrl) = 0) and change = '1' then
		mb <= '1';
		change_needed <= total;
	end if;
	if money = '1' then
		change_needed <= std_logic_vector(unsigned(total) - unsigned(price));
	end if;

	for i in 1 to 72 loop				--Sending a signal to the selected item to be collected
		collect(i) <= collect_ctrl(i) AND money AND change;
	end loop;
	
	end process;

extra3 <= total;
end selectarch;